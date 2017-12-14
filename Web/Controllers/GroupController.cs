using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Core.Specifications;
using Socona.ImVehicle.Web.ViewModels;
using Socona.ImVehicle.Infrastructure.Specifications;

namespace Socona.ImVehicle.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly UserManager<VehicleUser> _userManager;
        public GroupController(IGroupService groupService, UserManager<VehicleUser> userManager)
        {
            _groupService = groupService;
            _userManager = userManager;
        }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> GetDropDownList(long? townId)
        {
            var gs = await _groupService.ListGroupsForTownEagerAsync(HttpContext.User, townId ?? -1);

            var list = gs.Select(t => new { Value = t.Id, Text = t.Name });
            return new JsonResult(list);

        }
    
        public async Task<IActionResult> LoadData(int? townId,int? page = 0, int? pageSize = 20)
        {
            ISpecification<GroupItem> canFetch = await Group4UserSpecification.CreateAsync(HttpContext.User, _userManager);

            if (townId != null)
            {
                var inGroup = new GroupsInTownSpecification(townId.Value);
                canFetch = canFetch.And(inGroup);
            }
            canFetch.Includes.Add(t => t.Drivers);
            canFetch.Includes.Add(t => t.SecurityPersons);
            canFetch.Includes.Add(t => t.Vehicles);
            canFetch.Includes.Add(t => t.UserFiles);
            canFetch.Includes.Add(t => t.Town);

            var startIdx = (page) * pageSize ?? 0;
            startIdx = Math.Max(0, startIdx);
            var groups = await _groupService.ListRangeAsync(canFetch, startIdx, pageSize ?? 0);
            var list = groups.Select(t => new GroupListViewModel(t));
            return new JsonResult(list);
        }
    }
}
