using Microsoft.EntityFrameworkCore;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.AD;
using DMS.CORE;
using DMS.CORE.Entities.AD;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using DMS.BUSINESS.Services.HUB;
using Common;

namespace DMS.BUSINESS.Services.AD
{
    public interface IAccountGroupService : IGenericService<TblAdAccountGroup, AccountGroupDto>
    {
    }

    public class AccountGroupService(AppDbContext dbContext, IMapper mapper, IHubContext<RefreshServiceHub> hubContext) : GenericService<TblAdAccountGroup, AccountGroupDto>(dbContext, mapper), IAccountGroupService
    {
        private readonly IHubContext<RefreshServiceHub> _hubContext = hubContext;

        public override async Task<PagedResponseDto> Search(BaseFilter filter)
        {
            try
            {
                var query = _dbContext.TblAdAccountGroup.Include(x=>x.Account_AccountGroups).AsQueryable();
                //query = query.AsNoTracking();
                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                {
                    query = query.Where(x =>
                        x.Name.Contains(filter.KeyWord)
                    );
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

        public override async Task Update(IDto dto)
        {
            try
            {
                if (dto is not AccountGroupUpdateDto realDto)
                {
                    Status = false;
                    MessageObject.Code = "0000";
                    return;
                }

                realDto.ListAccountGroupRight.RemoveAll(x => x.RightId == "R");

                var entityInDB = await _dbContext.TblAdAccountGroup
                    .Where(x => x.Id == realDto.Id)
                    .Include(x => x.Account_AccountGroups)
                    .Include(x => x.ListAccountGroupRight)
                    .FirstOrDefaultAsync();

                if (entityInDB == null)
                {
                    Status = false;
                    MessageObject.Code = "2003";
                    return;
                }
                entityInDB.ListAccountGroupRight.Clear();
                _mapper.Map(dto, entityInDB);
                await _dbContext.SaveChangesAsync();
                if (this.Status)
                {
                    await _hubContext.Clients.Groups(realDto.Id.ToString()).SendAsync(SignalRMethod.RIGHT.ToString(), realDto.Id.ToString());
                }
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
            }
        }

        public override async Task<AccountGroupDto> GetById(object id)
        {
            try
            {
                var entity = await _dbContext.TblAdAccountGroup
                    .Where(x => x.Id == (Guid)id)
                    .Include(x => x.Account_AccountGroups)
                    .ThenInclude(x => x.Account)
                    .Include(x => x.ListAccountGroupRight)
                    .FirstOrDefaultAsync();

                var result = _mapper.Map<AccountGroupDto>(entity);

                // Lấy danh sách tất cả các quyền
                var lstNode = new List<RightDto>();
                var rootNode = new RightDto() { Id = "R", PId = "-R", Name = "Danh sách quyền trong hệ thống" };
                lstNode.Add(rootNode);

                var lstAllRight = await _dbContext.TblAdRight.Where(x=>x.Id != "R").OrderBy(x => x.OrderNumber).ToListAsync();
                if (result.ListAccountGroupRight.Count > 0)
                {
                    rootNode.IsChecked = true;
                }
                foreach (var right in lstAllRight)
                {
                    var node = new RightDto() { Id = right.Id, Name = right.Name, PId = right.PId };
                    if (result.ListAccountGroupRight.Any(x => x.RightId == right.Id))
                    {
                        node.IsChecked = true;
                    }
                    lstNode.Add(node);
                }

                var nodeDict = lstNode.ToDictionary(n => n.Id);
                foreach (var item in lstNode)
                {
                    if (item.PId == "-R" || !nodeDict.TryGetValue(item.PId, out RightDto parentNode))
                    {
                        continue;
                    }

                    parentNode.Children ??= [];
                    parentNode.Children.Add(item);
                }

                result.TreeRight = rootNode;

                return result;
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
