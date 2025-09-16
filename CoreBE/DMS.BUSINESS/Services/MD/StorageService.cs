using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.MD;
using DMS.CORE;
using DMS.CORE.Entities.MD;
using DMS.CORE.Entities.MT;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Services.MD
{
    public interface IStorageService : IGenericService<TblMdStorage, StorageDto>
    {
        Task<IList<StorageDto>> GetAll(BaseMdFilter filter);

        Task<List<StorageDto>> ImportExcel(IFormFile file);
        Task UpdateInfo(StorageDto data);
            //Task CreateInfo(StorageDto data);
            Task DeleteInfo(string id );
    }
    public class StorageService(AppDbContext dbContext, IMapper mapper) : GenericService<TblMdStorage, StorageDto>(dbContext, mapper), IStorageService
    {
        public override async Task<PagedResponseDto> Search(BaseFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdStorage.AsQueryable();
                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x => x.Code.ToString().Contains(filter.KeyWord) || x.Name.Contains(filter.KeyWord));
                }
                if (filter.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == filter.IsActive);
                }
                return await Paging(query, filter);

            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }


        public async Task<IList<StorageDto>> GetAll(BaseMdFilter filter)
        {
            try
            {
                var query = _dbContext.TblMdStorage.AsQueryable();
                if (filter.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == filter.IsActive);
                }
                return await base.GetAllMd(query, filter);
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }
        //public async Task CreateInfo(StorageDto data)
        //{
        //    try
        //    {
        //        var code = Guid.NewGuid().ToString();
                
        //        data.ID = code;
        //        _dbContext.TblMdStorage.Add(_mapper.Map<TblMdStorage>(data));
        //        await _dbContext.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        Status = false;
        //        Exception = ex;
        //    }
        //}
        public async Task UpdateInfo(StorageDto data)
        {
            try
            {
               
                _dbContext.TblMdStorage.Update(_mapper.Map<TblMdStorage>(data));

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
            }
        }
        public async Task DeleteInfo(string id)
        {
            try
            {
                var entity = await _dbContext.TblMdStorage.FindAsync(id);
                if (entity != null)
                {
                    _dbContext.TblMdStorage.Remove(entity);
                    await _dbContext.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
            }
        }
        public async Task<List<StorageDto>> ImportExcel(IFormFile file)
        {

            if (file == null || file.Length == 0)
            {
                // Nếu file rỗng thì ném exception hoặc trả null
                throw new ArgumentException("File rỗng");
            }


            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var memStream = new MemoryStream();
            await file.CopyToAsync(memStream);
            memStream.Position = 0;


            using var package = new ExcelPackage(memStream);
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
            if (worksheet == null)
            {
                throw new Exception("Không tìm thấy sheet trong file Excel");
            }

            // 5. Xác định số dòng dữ liệu
            var rowCount = worksheet.Dimension.End.Row;

            // 6. Danh sách để chứa các bản ghi mới
            var newProducts = new List<TblMdStorage>();

            // 7. Vòng lặp đọc từng dòng trong file Excel (bỏ dòng tiêu đề)
            for (int row = 2; row <= rowCount; row++) // giả sử dòng 1 là tiêu đề
            {
                // Đọc từng cột trong file Excel theo thứ tự
                var code = worksheet.Cells[row, 1].Text?.Trim(); // Cột A: Mã hàng hóa
                var name = worksheet.Cells[row, 2].Text?.Trim(); // Cột B: Tên hàng hóa
                

                // Nếu tất cả đều rỗng => bỏ qua dòng đó
                if (string.IsNullOrEmpty(code) && string.IsNullOrEmpty(name))
                {
                    continue;
                }
                // Kiểm tra xem sản phẩm đã tồn tại chưa (theo Code)
                var existingStorage = await _dbContext.TblMdStorage
                    .FirstOrDefaultAsync(x => x.Code == code);

                if (existingStorage == null)
                {

                    var entity = new TblMdStorage
                    {
                        ID = Guid.NewGuid().ToString(),
                        Code = code,
                        Name = name,
                   
                        IsActive = true, // mặc định active
                    };
                    newProducts.Add(entity);
                }
                else
                {

                    throw new Exception($"Đã có đơn vị tồn tại trong hệ thống");
                }
            }

            // Thêm danh sách bản ghi mới vào DB
            if (newProducts.Any())
            {
                _dbContext.TblMdStorage.AddRange(newProducts);
                await _dbContext.SaveChangesAsync();
            }
            var dtos = _mapper.Map<List<StorageDto>>(newProducts);

            return dtos;
        }
    }
}
