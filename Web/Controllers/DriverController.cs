using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImVehicleCore.Data;
using ImVehicleCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("[controller]/[action]")]
    public class DriverController : Controller
    {


        private readonly ImVehicleCore.Data.VehicleDbContext _context;
        private readonly UserManager<VehicleUser> _userManager;
        public DriverController(VehicleDbContext context, UserManager<VehicleUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> GetDropDownList(long? townId, long? groupId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            IEnumerable<DriverItem> drivers = new List<DriverItem>();
            if (!(await _userManager.IsInRoleAsync(user, "TownManager")) || user.TownId == townId)
            {
                drivers = _context.Drivers.Where(t => t.TownId == townId || t.GroupId == t.GroupId);

            }

            var list = drivers.Select(t => new { Value = t.Id, Text = (t.GroupId == groupId ? "*" : "") + t.Name + " (" + t.IdCardNumber + ")" });
            return new JsonResult(list);

        }
    }
}