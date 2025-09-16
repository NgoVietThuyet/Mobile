using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Dtos.AD;
using DMS.CORE;
using DMS.CORE.Entities.AD;
using Microsoft.EntityFrameworkCore;

namespace DMS.BUSINESS.Services.AD
{
    public interface IMenuService : IGenericService<TblAdMenu, MenuDto>
    {
        Task<MenuDto> BuildDataForTree();
        Task UpdateOrderTree(MenuDto moduleDto);
        Task<MenuDto> GetMenuOfUser(string userName);
        Task<MenuDto> Delete(string code);
        Task<MenuDetailDto> GetMenuWithTreeRight(object id);
    }

    public class MenuService(AppDbContext dbContext, IMapper mapper) : GenericService<TblAdMenu, MenuDto>(dbContext, mapper), IMenuService
    {

        /// <summary>
        /// Dựng cấu trúc nested tree
        /// </summary>
        /// <returns></returns>
        public async Task<MenuDto> BuildDataForTree()
        {
            var lstNode = new List<MenuDto>();
            var rootNode = new MenuDto() { Id = "MNU", PId = "-MNU", Name = "Danh sách menu", Title = "MNU - Danh sách menu", Expanded = true, Key = "MNU" };
            lstNode.Add(rootNode);

            var lstAllMenu = await _dbContext.TblAdMenu.OrderBy(x => x.OrderNumber).ToListAsync();
            foreach (var menu in lstAllMenu)
            {
                var node = new MenuDto()
                {
                    Id = menu.Id,
                    Name = menu.Name,
                    PId = menu.PId,
                    OrderNumber = menu.OrderNumber,
                    Icon = menu.Icon,
                    Url = menu.Url,
                    Title = $"{menu.Id} - {menu.Name}",
                    Key = menu.Id,
                    IsActive = menu.IsActive,
                    Expanded = true,
                };
                lstNode.Add(node);
            }
            var nodeDict = lstNode.ToDictionary(n => n.Id);
            foreach (var item in lstNode)
            {
                if (item.PId == "-MNU" || !nodeDict.TryGetValue(item.PId, out MenuDto parentNode))
                {
                    continue;
                }

                parentNode.Children ??= [];
                parentNode.Children.Add(item);
            }
            return rootNode;

        }

        public async Task UpdateOrderTree(MenuDto moduleDto)
        {
            try
            {
                if (string.IsNullOrEmpty(moduleDto.PId))
                {
                    Status = false;
                    MessageObject.Code = "1012";
                    return;
                }

                var lstModuleDto = new List<MenuDto>();
                var lstModuleUpdate = new List<TblAdMenu>();

                ConvertNestedToList(moduleDto, ref lstModuleDto);
                if (moduleDto.Children == null || moduleDto.Children.Count == 0)
                {
                    return;
                }
                var numberOrder = 1;
                foreach (var item in lstModuleDto)
                {
                    var module = _mapper.Map<TblAdMenu>(item);
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

        public async Task<MenuDto> GetMenuOfUser(string userName)
        {
            var lstNode = new List<MenuDto>();
            var rootNode = new MenuDto() { Id = "MNU", PId = "-MNU", Name = "Danh sách menu", Title = "MNU - Danh sách menu", Expanded = true, Key = "MNU" };
            lstNode.Add(rootNode);

            var lstRightOfUser = await GetRightOfUser(userName);
            var lstAllMenu = await _dbContext.TblAdMenu.Include(x => x.RightReferences)
                .Where(x => x.RightReferences.Any(y => lstRightOfUser.Contains(y.RightId)) || x.IsActive == true).OrderBy(x => x.OrderNumber).ToListAsync();
            foreach (var menu in lstAllMenu)
            {
                var node = new MenuDto()
                {
                    Id = menu.Id,
                    Name = menu.Name,
                    PId = menu.PId,
                    OrderNumber = menu.OrderNumber,
                    Icon = menu.Icon,
                    Url = menu.Url,
                    Title = $"{menu.Id} - {menu.Name}",
                    Key = menu.Id,
                    IsActive = menu.IsActive,
                    Expanded = true,
                };
                lstNode.Add(node);
            }
            var nodeDict = lstNode.ToDictionary(n => n.Id);
            foreach (var item in lstNode)
            {
                if (item.PId == "-MNU" || !nodeDict.TryGetValue(item.PId, out MenuDto parentNode))
                {
                    continue;
                }

                parentNode.Children ??= [];
                parentNode.Children.Add(item);
            }
            return rootNode;
        }


        private static void ConvertNestedToList(MenuDto node, ref List<MenuDto> lstNodeFlat)
        {
            if (node.Id != "MNU")
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

        public async Task<MenuDto> Delete(string code)
        {
            try
            {

                var codeString = code.ToString();
                var query = _dbContext.Set<TblAdMenu>().AsQueryable();

                query = query.Where(x => x.PId == codeString);

                var recordsWithSamePid = await query.ToListAsync();

                if (recordsWithSamePid.Count == 0)
                {
                    var recordToDelete = await _dbContext.Set<TblAdMenu>().FirstOrDefaultAsync(x => x.Id == codeString);

                    if (recordToDelete != null)
                    {
                        _dbContext.Remove(recordToDelete);
                        await _dbContext.SaveChangesAsync();
                    }
                    return _mapper.Map<MenuDto>(recordToDelete);
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

        public override async Task Update(IDto dto)
        {
            if (dto is MenuUpdateDto model)
            {
                var currentObj = await _dbContext.TblAdMenu.Include(x => x.RightReferences).FirstOrDefaultAsync(x => model.Id == x.Id);
                await base.UpdateWithListInside(dto, currentObj);
            }
        }

        public async Task<MenuDetailDto> GetMenuWithTreeRight(object id)
        {
            var data = await _dbContext.TblAdMenu.Include(x => x.RightReferences).FirstOrDefaultAsync(x => x.Id == id as string);

            if (data == null) return null;

            var lstNode = new List<RightDto>();
            var rootNode = new RightDto() { Id = "R", PId = "-R", Name = "Danh sách quyền trong hệ thống" };
            lstNode.Add(rootNode);

            var lstAllRight = await _dbContext.TblAdRight.Where(x => x.Id != "R").OrderBy(x => x.OrderNumber).ToListAsync();

            var lstRightInMenu = data.RightReferences.Select(x=>x.RightId).ToList();

            if (data.RightReferences.Count > 0)
            {
                rootNode.IsChecked = true;
            }
            foreach (var right in lstAllRight)
            {
                var node = new RightDto() { 
                    Id = right.Id, 
                    Name = right.Name, 
                    PId = right.PId,
                    OrderNumber = right.OrderNumber,
                    Title = $"{right.Id} - {right.Name}",
                    Key = right.Id,
                    IsActive = right.IsActive,
                    Expanded = true,
                };
                if (lstRightInMenu.Contains(right.Id))
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

            var result = _mapper.Map<MenuDetailDto>(data);
            result.TreeRight = rootNode;

            return result;
        }

        private async Task<List<string>> GetRightOfUser(string userName)
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
    }
}
