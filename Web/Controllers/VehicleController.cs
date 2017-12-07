using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Socona.ImVehicle.Web.ViewModels;
using Socona.ImVehicle.Web.ViewModels.Specifications;

namespace Socona.ImVehicle.Web.Controllers
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
    }
}