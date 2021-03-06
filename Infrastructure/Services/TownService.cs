﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Core.Specifications;
using Microsoft.AspNetCore.Identity;

namespace Socona.ImVehicle.Core.Services
{
    public class TownService : ITownService
    {
        private ITownRepository _townRepository;
        private IGroupRepository _groupService;
        private UserManager<VehicleUser> _userManager;
        private RoleManager<VehicleRole> _roleManager;

        public TownService(
            ITownRepository townRepository,
            IGroupRepository groupService,
            UserManager<VehicleUser> userManager,
            RoleManager<VehicleRole> roleManager)
        {
            this._townRepository = townRepository;
            this._groupService = groupService;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task<List<long>> GetAvailableTownIdsAsync(ClaimsPrincipal user)
        {
            var vUser = await _userManager.GetUserAsync(user);
            if (vUser != null)
            {

                if (await _userManager.IsInRoleAsync(vUser, "TownManager"))
                {
                    var townId = vUser.TownId;
                    if (townId != null)
                    {
                        return new List<long> { townId.Value };
                    }
                }
                else
                {
                    return (await _townRepository.ListAllEagerAsync()).Select(t => t.Id).ToList();

                }
            }
            return new List<long>();

        }
        public async Task<List<TownItem>> ListForUser(ClaimsPrincipal user, ISpecification<TownItem> specification)
        {
            var vUser = await _userManager.GetUserAsync(user);
            if (vUser == null)
            {
                return new List<TownItem>();
            }
            if (await _userManager.IsInRoleAsync(vUser, "TownManager"))
            {
                var townId = vUser.TownId;
                if (townId != null)
                {
                    return new List<TownItem>() { await _townRepository.GetByIdEagerAsync(townId.Value) }.Where(specification.Criteria.Compile()).ToList();
                }
            }
            else
            {
                return (await _townRepository.ListAllEagerAsync()).Where(specification.Criteria.Compile()).ToList(); ;
            }
            return new List<TownItem>();
        }

        public async Task<List<TownItem>> GetAvailableTownsEagerAsync(ClaimsPrincipal user)
        {
            Town4UserSpecification canFetch = await Town4UserSpecification.CreateAsync(user, _userManager);

            canFetch.Includes.Add(t => t.Drivers);
            canFetch.Includes.Add(t => t.Groups);
            canFetch.Includes.Add(t => t.Vehicles);

            return await _townRepository.ListAsync(canFetch);

        }
        public async Task<TownItem> GetByIdAsync(long id)
        {
            return await _townRepository.GetByIdAsync(id);
        }
        public async Task<List<TownItem>> ListAsync(ISpecification<TownItem> specification)
        {
            return await _townRepository.ListAsync(specification);
        }
    }
}
