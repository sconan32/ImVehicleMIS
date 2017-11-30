using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using ImVehicleCore.Data;
using ImVehicleCore.Interfaces;
using ImVehicleCore.Specifications;
using Microsoft.AspNetCore.Identity;

namespace ImVehicleCore.Data
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
        


        public async Task<List<TownItem>> GetAvailableTownsEagerAsync(ClaimsPrincipal user)
        {

           
            var vUser = await _userManager.GetUserAsync(user);
            if(vUser==null)
            {
                return new List<TownItem>();
            }
            if( await _userManager.IsInRoleAsync(vUser,"TownManager"))
            {
                var townId = vUser.TownId;
                if (townId != null)
                {
                    return new List<TownItem>() { await _townRepository.GetByIdEagerAsync(townId.Value) };
                }
            }
            else
            {
                return await _townRepository.ListAllEagerAsync();
            }
            return new List<TownItem>();

        }

        
    }
}
