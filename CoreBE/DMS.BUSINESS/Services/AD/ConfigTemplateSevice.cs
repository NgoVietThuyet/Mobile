using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.AD;
using DMS.BUSINESS.Dtos.MD;
using DMS.CORE;
using DMS.CORE.Entities.AD;
using Microsoft.EntityFrameworkCore;

namespace DMS.BUSINESS.Services.AD
{
    public interface IConfigTemplateService : IGenericService<TblAdConfigTemplate, ConfigTemplateDto>
    {
        Task<IList<ConfigTemplateDto>> GetAll(BaseMdFilter filter);
        Task<PagedResponseDto> Search(BaseFilter filter);
    }

    public class ConfigTemplateService(AppDbContext dbContext, IMapper mapper) : GenericService<TblAdConfigTemplate, ConfigTemplateDto>(dbContext, mapper), IConfigTemplateService
    {
        public override async Task<PagedResponseDto> Search(BaseFilter filter)
        {
            try
            {
                var query = _dbContext.TblAdConfigTemplate.AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                    x.Type.Contains(filter.KeyWord));
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
        public async Task<IList<ConfigTemplateDto>> GetAll(BaseMdFilter filter)
        {
            try
            {
                var query = _dbContext.TblAdConfigTemplate.AsQueryable();
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
    }
}
