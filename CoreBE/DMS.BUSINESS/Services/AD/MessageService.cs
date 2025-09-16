using AutoMapper;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.AD;
using DMS.CORE;
using DMS.CORE.Entities.AD;
using Common;
using Microsoft.EntityFrameworkCore;


namespace DMS.BUSINESS.Services.AD
{

    public interface IMessageService : IGenericService<TblAdMessage, MessageDto>
    {
        Task<IList<MessageDto>> GetAll(BaseMdFilter filter);
    }
    public class MessageService(AppDbContext dbContext, IMapper mapper) : GenericService<TblAdMessage, MessageDto>(dbContext, mapper), IMessageService
    {
        public override async Task<PagedResponseDto> Search(BaseFilter filter)
        {
            try
            {
                var query = _dbContext.TblAdMessage.AsQueryable();
                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.Code.Contains(filter.KeyWord) ||
                        x.Lang.Contains(filter.KeyWord) ||
                        x.Value.Contains(filter.KeyWord)
                    );
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

        public async Task<IList<MessageDto>> GetAll(BaseMdFilter filter)
        {
            try
            {
                var query = _dbContext.TblAdMessage.AsQueryable();
                if (filter.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == filter.IsActive);
                }
                query = query.OrderByDescending(x => x.CreateDate);
                return _mapper.Map<IList<MessageDto>>(await query.ToListAsync());
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
