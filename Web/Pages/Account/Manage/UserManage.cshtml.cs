using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Infrastructure.Interfaces;
using Socona.ImVehicle.Infrastructure.ViewModels;

namespace Socona.ImVehicle.Web.Pages.Account.Manage
{
    public class UserManageModel : PageModel
    {
        private readonly VehicleDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly ITownService _townService;
        private readonly IGroupService _groupService;
        private readonly UserManager<VehicleUser> _userManager;

        public UserManageModel(VehicleDbContext context, IAuthorizationService authorizationService, ITownService townService,
            IGroupService groupService, UserManager<VehicleUser> userManager)
        {
            _context = context;
            _authorizationService = authorizationService;
            _townService = townService;
            _groupService = groupService;
            _userManager = userManager;
        }

        public IList<UserViewModel> UserList { get; set; }

        [Authorize(Roles = "Admins,GlobalVisitor,TownManager")]
        public async Task OnGetAsync()
        {
            UserList = new List<UserViewModel>();
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(user, "GlobalVisitor") ||
                await _userManager.IsInRoleAsync(user, "Admins"))
            {
                var global = await _userManager.GetUsersInRoleAsync("GlobalVisitor");
                await AddUsers(global, "区管理员");
            }
            if (await _userManager.IsInRoleAsync(user, "GlobalVisitor") ||
               await _userManager.IsInRoleAsync(user, "Admins"))
            {
                var tm = await _userManager.GetUsersInRoleAsync("TownManager");
                if (await _userManager.IsInRoleAsync(user, "TownManager"))
                {
                    tm = tm.Where(t => t.TownId == user.TownId).ToList();
                }
                await AddUsers(tm, "街道管理员");
            }
            if (await _userManager.IsInRoleAsync(user, "GlobalVisitor") ||
              await _userManager.IsInRoleAsync(user, "Admins")||
              await _userManager.IsInRoleAsync(user, "TownManager"))
            {



                var gm = await _userManager.GetUsersInRoleAsync("GroupManager");
                if (await _userManager.IsInRoleAsync(user, "TownManager"))
                {
                    gm = gm.Where(t => t.TownId == user.TownId).ToList();
                }

                await AddUsers(gm, "安全组管理员");
            }
        }
        private async Task AddUsers(IList<VehicleUser> users, string roleName)
        {
            foreach (var user in users)
            {
                var vm = new UserViewModel(user);
                if (user.GroupId != null)
                {
                    vm.GroupName = (await _groupService.GetByIdAsync(user.GroupId.Value)).Name;
                }
                if (user.TownId != null)
                {
                    vm.TownName = (await _townService.GetByIdAsync(user.TownId.Value)).Name;
                }
                vm.Type = roleName;
                UserList.Add(vm);
            }
        }
    }
}