using Common;
using DMS.CORE.Common;

namespace DMS.BUSINESS.Common
{
    public interface IGenericService<TEntity, TDto> : IBaseService where TEntity : BaseEntity where TDto : class
    {
        Task<PagedResponseDto> Search(BaseFilter filter);
        Task<IList<TDto>> GetAll();
        Task<IList<TDto>> GetAllActive();
        Task<TDto> GetById(object id);
        Task<TDto> Add(IDto dto);
        Task Update(IDto dto);
        Task Delete(object code);
        Task<PagedResponseDto> Paging(IQueryable<TEntity> query, BaseFilter filter);
    }
}
