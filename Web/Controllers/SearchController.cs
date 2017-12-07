using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("[controller]/[action]")]
    public class SearchController : Controller
    {
        private readonly VehicleDbContext _context;
        private readonly UserManager<VehicleUser> _userManager;
        public SearchController(VehicleDbContext context, UserManager<VehicleUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> GetDropDownList(long? townId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            IEnumerable<DriverItem> drivers = new List<DriverItem>();
            if (!(await _userManager.IsInRoleAsync(user, "TownManager") || user.TownId == townId))
            {
                drivers = _context.Drivers.Where(t => t.TownId == townId);

            }

            var list = drivers.Select(t => new { Value = t.Id, Text = t.Name });
            return new JsonResult(list);

        }


        public IActionResult Index(string queryString)
        {
            if (!string.IsNullOrEmpty(queryString))
            {
                Dictionary<string, string> mapStringToTable = new Dictionary<string, string> {
                { "街道", "Town" },{"镇", "Town" },
                { "安全单位", "Group" },{"单位", "Group" },{ "公司","Group" },{"村", "Group" },{ "企业", "Group" },
                { "车辆", "Vehicle" },{"车", "Vehicle" },
                { "驾驶员", "Driver" },{"司机","Driver" },
                { "文件", "UserFile"},
                { "安全员","SecurePerson"},
            };
                string[] splitString = queryString.Split(':', '：');

                string routeFolder = null;
                if (splitString.Length == 2 && mapStringToTable.TryGetValue(splitString[0], out routeFolder))
                {
                    return Json(new { RequestUrl = "/" + routeFolder + "/Query?queryString=" + splitString[1] });
                }
            }
            return NoContent();
        }
    }
}