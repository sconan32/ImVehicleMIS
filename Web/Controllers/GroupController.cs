using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ImVehicleCore.Interfaces;

namespace Web.Controllers
{
    [Route("[controller]/[action]")]
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;
        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> GetDropDownList(long? townId)
        {
            var gs = await _groupService.ListGroupsForTownEagerAsync(HttpContext.User, townId ?? -1);

            var list = gs.Select(t => new { Value = t.Id, Text = t.Name });
            return new JsonResult(list);

        }
    }
}
