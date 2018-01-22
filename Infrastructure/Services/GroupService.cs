using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Socona.ImVehicle.Core.Services
{
    public class GroupService : IGroupService
    {
        private IGroupRepository _groupService;
        private UserManager<VehicleUser> _userManager;

        public GroupService(
            IGroupRepository groupService,
            UserManager<VehicleUser> userManager)
        {
            this._groupService = groupService;
            this._userManager = userManager;
        }

        public Task<List<GroupItem>> ListRangeAsync(ISpecification<GroupItem> canFetch, int start, int count)
        {
            return _groupService.ListRangeAsync(canFetch, start, count);
        }

        public async Task<List<GroupItem>> ListAwailableGroupEagerAsync(ClaimsPrincipal claim)
        {
            var vUser = await _userManager.GetUserAsync(claim);
            if (vUser != null)
            {
                if (await _userManager.IsInRoleAsync(vUser, "TownManager"))
                {
                    var townId = vUser.TownId;
                    if (townId != null)
                    {
                        return await _groupService.GetGroupsOfTown(townId.Value);
                    }
                }
                else
                {
                    return await _groupService.ListAllWithVehiclesAsync();
                }
            }
            return new List<GroupItem>();
        }

        public async Task<List<GroupItem>> ListGroupsForTownEagerAsync(ClaimsPrincipal claim, long townId)
        {
            var vUser = await _userManager.GetUserAsync(claim);
            if (vUser != null)
            {
                if (await _userManager.IsInRoleAsync(vUser, "TownManager"))
                {
                    var utownId = vUser.TownId;
                    if (utownId != null && utownId == townId)
                    {
                        return await _groupService.GetGroupsOfTown(townId);
                    }
                }
                else
                {
                    return await _groupService.GetGroupsOfTown(townId);
                }
            }
            return new List<GroupItem>();
        }
        public async Task<List<GroupItem>> ListAsync(ISpecification<GroupItem> canFetch)
        {

            return await _groupService.ListAsync(canFetch) ;
        }

        public async Task<GroupItem> GetByIdAsync(long id)
        {
            return await _groupService.GetByIdAsync(id);
        }

    }
}
