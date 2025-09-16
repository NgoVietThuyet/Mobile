using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.AD;
using DMS.CORE;
using DMS.CORE.Entities.AD;
using Microsoft.EntityFrameworkCore;

namespace DMS.BUSINESS.Services.AD
{
    public interface IRightService : IGenericService<TblAdRight, RightDto>
    {
        Task<RightDto> BuildDataForTree();
        Task UpdateOrderTree(RightDto moduleDto);
        Task<bool> CheckRight(string right, string UserName);
        Task<List<string>> GetRightOfUser(string userName);
        Task<RightDto> Delete(string code);
    }
    public class RightService(AppDbContext dbContext, IMapper mapper) : GenericService<TblAdRight, RightDto>(dbContext, mapper), IRightService
    {
        public async Task<RightDto> BuildDataForTree()
        {
            var lstNode = new List<RightDto>();
            RightDto rootNode = new() { Id = "R", PId = "-R", Name = "Danh sách quyền trong hệ thống", Title = "R. Danh sách quyền trong hệ thống", Key = "R", Expanded = true, IsActive = true };
            lstNode.Add(rootNode);

            var lstAllRight = (await GetAll()).OrderBy(x => x.OrderNumber).ToList();
            foreach (var right in lstAllRight.Where(x => x.Id != "R"))
            {
                var node = new RightDto() { Id = right.Id, PId = right.PId, Name = right.Name, OrderNumber = right.OrderNumber, Title = $"{right.Id} {right.Name}", Key = right.Id, IsActive = right.IsActive };
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
            return rootNode;
        }

        public async Task UpdateOrderTree(RightDto moduleDto)
        {
            try
            {
                var lstModuleDto = new List<RightDto>();
                var lstModuleUpdate = new List<TblAdRight>();

                ConvertNestedToList(moduleDto, ref lstModuleDto);
                if (moduleDto.Children == null || moduleDto.Children.Count == 0)
                {
                    return;
                }
                var numberOrder = 1;
                foreach (var item in lstModuleDto)
                {
                    var module = _mapper.Map<TblAdRight>(item);
                    module.OrderNumber = numberOrder++;
                    lstModuleUpdate.Add(module);
                }
                _dbContext.UpdateRange(lstModuleUpdate);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
            }
        }

        public async Task<bool> CheckRight(string right, string UserName)
        {
            if (string.IsNullOrEmpty(right)) { return true; }
            if (string.IsNullOrEmpty(UserName)) { return false; }
            var lstRightOfUser = await GetRightOfUser(UserName);
            var lstRightCheck = right.Split(',');
            foreach (var item in lstRightCheck)
            {
                if (lstRightOfUser.Contains(item)) { return true; }
            }
            return false;
        }

        public async Task<List<string>> GetRightOfUser(string userName)
        {
            var user = await _dbContext.TblAdAccount.Include(x => x.Account_AccountGroups)
                .ThenInclude(x => x.AccountGroup).ThenInclude(x => x.ListAccountGroupRight)
                .Include(x => x.AccountRights)
                .FirstOrDefaultAsync(x => x.UserName == userName);

            if (user == null) return [];

            var listRightOfUser = new List<string>();

            var lstRightInGroup = user.Account_AccountGroups
                .Select(x => x.AccountGroup)
                .SelectMany(x => x.ListAccountGroupRight)
                .Select(x => x.RightId).ToList();

            var listRightOutGroup = user.AccountRights.Where(x => x.IsAdded == true).Select(x => x.RightId).ToList();

            var listRightOutGroupRemoved = user.AccountRights.Where(x => x.IsRemoved == true).Select(x => x.RightId).ToList();


            var result = listRightOfUser.Concat(lstRightInGroup).Concat(listRightOutGroup).Distinct().ToList();

            result.RemoveAll(x => listRightOutGroupRemoved.Contains(x));

            return result;
        }

        private static void ConvertNestedToList(RightDto node, ref List<RightDto> lstNodeFlat)
        {
            if (node.Id != "R")
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
        public override Task<RightDto> Add(IDto dto)
        {
            if (dto is not RightDto model)
            {
                Status = false;
                MessageObject.Code = "0000";
                return null;
            }
            if (string.IsNullOrWhiteSpace(model.PId))
            {
                model.PId = "R";
            }
            return base.Add(dto);
        }


        public async Task<RightDto> Delete(string code)
        {
            try
            {
                var codeString = code.ToString();
                var query = _dbContext.Set<TblAdRight>().AsQueryable(); // Sử dụng _dbContext.Set<TblAdRight>() để truy cập bảng TblAdRight

                query = query.Where(x => x.PId == codeString);

                var recordsWithSamePid = await query.ToListAsync();

                if (recordsWithSamePid.Count == 0)
                {
                    var recordToDelete = await _dbContext.Set<TblAdRight>().FirstOrDefaultAsync(x => x.Id == codeString); // Sử dụng _dbContext.Set<TblAdRight>() để truy cập bảng TblAdRight

                    if (recordToDelete != null)
                    {
                        _dbContext.Remove(recordToDelete);
                        await _dbContext.SaveChangesAsync();
                    }
                    return _mapper.Map<RightDto>(recordToDelete);
                }
                return null;
            }
            catch (Exception ex)
            {
                Status = false;
                Exception = ex;
                return null;
            }
        }

        public override Task Update(IDto dto)
        {
            var obj = dto as RightDto;

            if (string.IsNullOrEmpty(obj.PId))
            {
                obj.PId = "R";
            }

            return base.Update(dto);
        }
    }
}
