using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.AD;
using DMS.CORE;
using DMS.CORE.Entities.AD;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Services.AD
{
    public interface IOrganizeService : IGenericService<tblAdOrganize, OrganizeDto>
    {
        Task<OrganizeDto> BuildDataForTree();
        Task UpdateOrderTree(OrganizeDto moduleDto);
        new Task<OrganizeDto> Delete(object code);
        Task<IList<OrganizeDto>> GetAll(BaseFilter filter);

        Task<OrganizeDto> BuildTreeForUser(string userName);

    }
    public class OrganizeService : GenericService<tblAdOrganize, OrganizeDto>, IOrganizeService
    {
        public OrganizeService(AppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
        public async Task<OrganizeDto> BuildDataForTree()
        {
            var lstNode = new List<OrganizeDto>();
            var rootNode = new OrganizeDto() { Id = "ORG", PId = "-ORG", Name = "Danh Sách Các Đơn Vị", Title = "ORG - Cây cấu trúc tổ chức", Key = "ORG" };
            lstNode.Add(rootNode);

            var lstAllOrganize = (await this.GetAll()).OrderBy(x => x.OrderNumber).ToList();
            foreach (var Organize in lstAllOrganize)
            {
                var node = new OrganizeDto()
                {
                    Id = Organize.Id,
                    Name = Organize.Name,
                    PId = Organize.PId,                                   
                    OrderNumber = Organize.OrderNumber,
                    Title = $"{Organize.Id}_{Organize.Name}",
                    IsChecked = Organize.IsChecked,
                    IsActive = Organize.IsActive,
                    Key = Organize.Id,
                };
                lstNode.Add(node);
            }
            var nodeDict = lstNode.ToDictionary(n => n.Id);
            foreach (var item in lstNode)
            {
                if (item.PId == "-ORG" || !nodeDict.TryGetValue(item.PId, out OrganizeDto parentNode))
                {
                    continue;
                }

                if (parentNode.Children == null)
                {
                    parentNode.Children = new List<OrganizeDto>();
                }
                parentNode.Children.Add(item);
            }
            return rootNode;
        }
        public async Task<OrganizeDto> BuildTreeForUser(string userName)
        {
           
            var userType = _dbContext.TblAdAccount.Where(x => x.UserName == userName).Select(x => x.AccountType).FirstOrDefault();
            if(userType == "ACT_UNIT")
            {
                var lstNode = new List<OrganizeDto>();
                var rootNode = new OrganizeDto() { Id = "ORG", PId = "-ORG", Name = "Danh Sách Các Đơn Vị", Title = "ORG - Cây cấu trúc tổ chức", Key = "ORG" };
                lstNode.Add(rootNode);
                var org = _dbContext.TblAdAccount.Where(x => x.UserName == userName).Select(x => x.OrganizeCode).FirstOrDefault();
                var lstAllOrganize = _dbContext.tblAdOrganize.Where(x => x.Id == org).ToList();
                var lstAllOrganizeForUser = _mapper.Map<List<OrganizeDto>>(lstAllOrganize);
                foreach (var Organize in lstAllOrganizeForUser)
                {
                    var node = new OrganizeDto()
                    {
                        Id = Organize.Id,
                        Name = Organize.Name,
                        PId = Organize.PId,
                        OrderNumber = Organize.OrderNumber,
                        Title = $"{Organize.Id}_{Organize.Name}",
                        IsChecked = Organize.IsChecked,
                        IsActive = Organize.IsActive,
                        Key = Organize.Id,
                    };
                    lstNode.Add(node);
                }
                var nodeDict = lstNode.ToDictionary(n => n.Id);
                foreach (var item in lstNode)
                {
                    if (item.PId == "-ORG" || !nodeDict.TryGetValue(item.PId, out OrganizeDto parentNode))
                    {
                        continue;
                    }

                    if (parentNode.Children == null)
                    {
                        parentNode.Children = new List<OrganizeDto>();
                    }
                    parentNode.Children.Add(item);
                }
                return rootNode;
            }
            else
            {

                var lstNode = new List<OrganizeDto>();
                var rootNode = new OrganizeDto() { Id = "ORG", PId = "-ORG", Name = "Danh Sách Các Đơn Vị", Title = "ORG - Cây cấu trúc tổ chức", Key = "ORG" };
                lstNode.Add(rootNode);

                var lstAllOrganize = (await this.GetAll()).OrderBy(x => x.OrderNumber).ToList();
                foreach (var Organize in lstAllOrganize)
                {
                    var node = new OrganizeDto()
                    {
                        Id = Organize.Id,
                        Name = Organize.Name,
                        PId = Organize.PId,
                        OrderNumber = Organize.OrderNumber,
                        Title = $"{Organize.Id}_{Organize.Name}",
                        IsChecked = Organize.IsChecked,
                        IsActive = Organize.IsActive,
                        Key = Organize.Id,
                    };
                    lstNode.Add(node);
                }
                var nodeDict = lstNode.ToDictionary(n => n.Id);
                foreach (var item in lstNode)
                {
                    if (item.PId == "-ORG" || !nodeDict.TryGetValue(item.PId, out OrganizeDto parentNode))
                    {
                        continue;
                    }

                    if (parentNode.Children == null)
                    {
                        parentNode.Children = new List<OrganizeDto>();
                    }
                    parentNode.Children.Add(item);
                }
                return rootNode;
            }
            
        }

        public async Task UpdateOrderTree(OrganizeDto moduleDto)
        {
            try
            {
                var lstModuleDto = new List<OrganizeDto>();
                var lstModuleUpdate = new List<tblAdOrganize>();

                this.ConvertNestedToList(moduleDto, ref lstModuleDto);
                if (moduleDto.Children == null || moduleDto.Children.Count == 0)
                {
                    return;
                }
                var numberOrder = 1;
                foreach (var item in lstModuleDto)
                {
                    var module = _mapper.Map<tblAdOrganize>(item);
                    module.OrderNumber = numberOrder++;
                    lstModuleUpdate.Add(module);
                }
                this._dbContext.UpdateRange(lstModuleUpdate);
                await this._dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
            }
        }

        private void ConvertNestedToList(OrganizeDto node, ref List<OrganizeDto> lstNodeFlat)
        {
            if (node.Id != "ORG")
            {
                lstNodeFlat.Add(node);
            }
            if (node.Children != null && node.Children.Count > 0)
            {
                foreach (var item in node.Children)
                {
                    ConvertNestedToList(item, ref lstNodeFlat);
                }
            }
        }
        public override Task<OrganizeDto> Add(IDto dto)
        {
            var model = dto as OrganizeDto;
            if (string.IsNullOrWhiteSpace(model.PId))
            {
                model.PId = "ORG";
            }
            return base.Add(dto);
        }


        public async new Task<OrganizeDto> Delete(object code)
        {
            try
            {
                var codeString = code.ToString();
                var query = _dbContext.Set<tblAdOrganize>().AsQueryable(); // Sử dụng _dbContext.Set<tblAdOrganize>() để truy cập bảng tblAdOrganize

                query = query.Where(x => x.PId == codeString);

                var recordsWithSamePid = await query.ToListAsync();

                if (recordsWithSamePid.Count == 0)
                {
                    var recordToDelete = await _dbContext.Set<tblAdOrganize>().FirstOrDefaultAsync(x => x.Id == codeString); // Sử dụng _dbContext.Set<tblAdOrganize>() để truy cập bảng tblAdOrganize

                    if (recordToDelete != null)
                    {
                        _dbContext.Remove(recordToDelete);
                        await _dbContext.SaveChangesAsync();
                    }
                    return _mapper.Map<OrganizeDto>(recordToDelete);
                }
                return null;
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return null;
            }
        }

        public async Task<IList<OrganizeDto>> GetAll(BaseFilter filter)
        {
            try
            {
                var query = this._dbContext.tblAdOrganize.AsQueryable();

                if (filter.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == filter.IsActive);
                }
                query = query.OrderBy(x => x.OrderNumber);
                return _mapper.Map<IList<OrganizeDto>>(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return null;
            }
        }

    }
}
