using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Web.ViewModels.Specifications;
using Socona.ImVehicle.Core.Specifications;

namespace Web.Pages.Group
{
    public class IndexModel : PageModel
    {
        private readonly IGroupService _groupService;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<VehicleUser> _userManager;

        public IndexModel(IGroupService groupService, IAuthorizationService authorizationService, UserManager<VehicleUser> userManager)
        {
            _groupService = groupService;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        public List<GroupListViewModel> Groups { get; set; }



        public async Task<bool> CanEdit()
        {
            var tm = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireTownManagerRole");
            var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await tm).Succeeded || (await admin).Succeeded;
        }
        public async Task<bool> IsAdmin()
        {
            var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await admin).Succeeded;
        }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task OnGetAsync()
        {
            var page = 0;
            var pageSize = 20;

            var specification = await Group4UserSpecification.CreateAsync(HttpContext.User, _userManager);
            specification.Includes.Add(t => t.Drivers);
            specification.Includes.Add(t => t.SecurityPersons);
            specification.Includes.Add(t => t.Vehicles);
            specification.Includes.Add(t => t.UserFiles);
            specification.Includes.Add(t => t.Town);

            var startIdx = (page) ;
            startIdx = Math.Max(0, startIdx);
            var groups = new List<GroupItem>(); //await _groupService.ListRangeAsync(specification, startIdx, pageSize );
            Groups = groups.Select(t => new GroupListViewModel(t)).ToList();
        }


        
    }
}
