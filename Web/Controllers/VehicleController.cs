using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImVehicleCore.Data;
using ImVehicleCore.Interfaces;
using ImVehicleCore.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;
using Web.ViewModels.Specifications;

namespace Web.Controllers
{
    [Route("[controller]/[action]")]
    public class VehicleController : Controller
    {

        private readonly IVehicleService _vehicleService;
        private readonly UserManager<VehicleUser> _userManager;
        public VehicleController(IVehicleService groupService, UserManager<VehicleUser> userManager)
        {
            _vehicleService = groupService;
            _userManager = userManager;
        }


        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> LoadData(int? page = 0, int? pageSize = 20)
        {
            var specification = await Vehicle4UserSpecification.CreateAsync(HttpContext.User, _userManager);
            specification.Includes.Add(t => t.Driver);
            specification.Includes.Add(t => t.Town);
            specification.Includes.Add(t => t.Group);

            var startIdx = (page) * pageSize ?? 0;
            startIdx = Math.Max(0, startIdx);
            var groups = await _vehicleService.ListRangeAsync(specification, startIdx, pageSize ?? 0);
            var list = groups.Select(t => new VehicleListViewModel(t));
            return new JsonResult(list);
        }
    }
}