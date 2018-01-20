using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Core.Specifications;
using Socona.ImVehicle.Infrastructure.Tools;
using Socona.ImVehicle.Web.ViewModels;

namespace Socona.ImVehicle.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class DriverController : Controller
    {


        private readonly VehicleDbContext _context;
        private readonly UserManager<VehicleUser> _userManager;
        private IAsyncRepository<DriverItem> _driverPepository;

        public DriverController(VehicleDbContext context, UserManager<VehicleUser> userManager, IAsyncRepository<DriverItem> driverPepository)
        {
            _context = context;
            _userManager = userManager;
            _driverPepository = driverPepository;
        }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> GetDropDownList(long? townId, long? groupId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            IEnumerable<DriverItem> drivers = new List<DriverItem>();
            if (await _userManager.IsInRoleAsync(user, "TownManager"))
            {
                if (user.TownId == townId)
                {
                    drivers = _context.Drivers.Where(t => t.TownId == townId || t.GroupId == groupId);
                }

            }
            else
            {
                drivers = _context.Drivers.Where(t => t.TownId == townId || t.GroupId == groupId);
            }

            var list = drivers.Select(t => new { Value = t.Id, Text = (t.GroupId == groupId ? "*" : "") + t.Name + " (" + t.IdCardNumber + ")" });
            return new JsonResult(list);

        }

        [Authorize(Roles = "GlobalVisitor,TownManager,Admins")]
        public async Task<IActionResult> LoadData(int? townId, int? page = 0, int? pageSize = 20)
        {
            ISpecification<DriverItem> canFetch = await Driver4UserSpecification.CreateAsync(HttpContext.User, _userManager);


            if (townId != null)
            {
                var inGroup = new DriverInTownSpecification(townId.Value);
                canFetch = canFetch.And(inGroup);
            }
            canFetch.Includes.Add(t => t.Vehicles);
            canFetch.Includes.Add(t => t.Town);
            canFetch.Includes.Add(t => t.Group);

            var startIdx = (page) * pageSize ?? 0;
            startIdx = Math.Max(0, startIdx);
            var groups = await _driverPepository.ListRangeAsync(canFetch, startIdx, pageSize ?? 0);
            var list = groups.Select(t => new DriverListViewModel(t));
            return new JsonResult(list);
        }

        [Authorize(Roles = "GlobalVisitor,TownManager,Admins")]
        public async Task<IActionResult> ExportExcel(long? townId, long? groupId)
        {
            ISpecification<DriverItem> canFetch = await Driver4UserSpecification.CreateAsync(HttpContext.User, _userManager);


            if (townId != null)
            {
                var inTown = new DriverInTownSpecification(townId.Value);
                canFetch = canFetch.And(inTown);
            }
            if (groupId != null)
            {
                var inGroup = new Specification<DriverItem>(t => t.GroupId == groupId);
                canFetch = canFetch.And(inGroup);
            }
            canFetch.Includes.Add(t => t.Vehicles);
            canFetch.Includes.Add(t => t.Town);
            canFetch.Includes.Add(t => t.Group);

            var items = await _driverPepository.ListAsync(canFetch);
            items = items.OrderBy(t => t.IsValid()).ThenBy(t => t.LicenseIssueDate).ToList();
            if (!items.Any())
            {
                return NoContent();
            }
            var sigStr = $"::{HttpContext.User.Identity.Name}::socona.imvehicle.driver.export?town={townId}&group={groupId}::{DateTime.Now.ToString("yyyyMMdd.HHmmss")}";
            var stream = ExcelHelper.ExportDrivers(items, sigStr);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"exported{DateTime.Now.ToString("yyyyMMdd.HHmmss")}.xlsx");
        }

    }
}