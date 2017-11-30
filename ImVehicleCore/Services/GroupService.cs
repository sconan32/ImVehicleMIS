﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ImVehicleCore.Data;
using ImVehicleCore.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ImVehicleCore.Services
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
    }
}
