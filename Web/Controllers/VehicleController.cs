using System;
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
    public class VehicleController : Controller
    {

        private readonly IVehicleService _vehicleService;
        private readonly UserManager<VehicleUser> _userManager;
        public VehicleController(IVehicleService vehicleService, UserManager<VehicleUser> userManager)
        {
            _vehicleService = vehicleService;
            _userManager = userManager;
        }



        public async Task<IActionResult> LoadData(int? townId, int? page = 0, int? pageSize = 20)
        {
            ISpecification<VehicleItem> canFetch = await Vehicle4UserSpecification.CreateAsync(HttpContext.User, _userManager);
            if (townId != null)
            {
                var inTown = new VehicleInTownSpecification(townId.Value);
                canFetch = canFetch.And(inTown);
            }


            canFetch.Includes.Add(t => t.Driver);
            canFetch.Includes.Add(t => t.Town);
            canFetch.Includes.Add(t => t.Group);

            var startIdx = (page) * pageSize ?? 0;
            startIdx = Math.Max(0, startIdx);
            var groups = await _vehicleService.ListRangeAsync(canFetch, startIdx, pageSize ?? 0);
            var list = groups.Select(t => new VehicleListViewModel(t));
            return new JsonResult(list);
        }

        [Authorize(Roles = "GlobalVisitor,TownManager,Admins")]
        public async Task<IActionResult> ExportExcel(long? townId, long? groupId)
        {
            ISpecification<VehicleItem> canFetch = await Vehicle4UserSpecification.CreateAsync(HttpContext.User, _userManager);


            if (townId != null)
            {
                var inTown = new VehicleInTownSpecification(townId.Value);
                canFetch = canFetch.And(inTown);
            }
            if (groupId != null)
            {
                var inGroup = new Specification<VehicleItem>(t => t.GroupId == groupId);
                canFetch = canFetch.And(inGroup);
            }
         
            canFetch.Includes.Add(t => t.Town);
            canFetch.Includes.Add(t => t.Group);
            canFetch.Includes.Add(t => t.Driver);
            var items = await _vehicleService.ListAsync(canFetch);
            items = items.OrderBy(t => t.IsValid()).ThenBy(t => t.YearlyAuditDate).ToList();
            if (!items.Any())
            {
                return NoContent();
            }
            var sigStr = $"::{HttpContext.User.Identity.Name}::socona.imvehicle.vehicle.export?town={townId}&group={groupId}::{DateTime.Now.ToString("yyyyMMdd.HHmmss")}";
            var stream = ExcelHelper.ExportVehicles(items, sigStr);
           
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"exported{DateTime.Now.ToString("yyyyMMdd.HHmmss")}.xlsx");
        }
    }
}