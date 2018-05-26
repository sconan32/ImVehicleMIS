using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Infrastructure.Extensions;
using Socona.ImVehicle.Web.ViewModels;

namespace Socona.ImVehicle.Web.Pages.Vehicle
{
    public class DetailsModel : PageModel
    {
        private readonly VehicleDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly ITownRepository _townRepository;
        IGroupRepository _groupService;


        public DetailsModel(VehicleDbContext context, ITownRepository townRepository, IGroupRepository groupService, IAuthorizationService authorizationService)
        {
            _townRepository = townRepository;
            _groupService = groupService;
            _authorizationService = authorizationService;
            _context = context;

        }
        public async Task<bool> CanEdit()
        {
            var tm = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireTownManagerRole");
            var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await tm).Succeeded || (await admin).Succeeded;
        }


        public VehicleViewModel VehicleItem { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .Include(t=>t.Group)
                .Include(t=>t.Town)
                .Include(t=>t.Driver)
                .Include(t=>t.ExtraImage1)
                .Include(t=>t.ExtraImage2)
                .Include(t=>t.ExtraImage3)
                .Include(t=>t.FrontImage)
                .Include(t=>t.RearImage)
                .Include(t=>t.LicenseImage)
                .Include(t=>t.GpsImage)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (vehicle == null)
            {
                return NotFound();
            }


            VehicleItem = new VehicleViewModel(vehicle);
          
            var nowDate = DateTime.Now.Date;
            VehicleItem.IsAuditValid = VehicleItem.YearlyAuditDate?.AddYears(1) >= nowDate;
            VehicleItem.IsInsuranceValid = vehicle.AuditExpiredDate?.AddYears(1) >= nowDate;
            VehicleItem.IsDumpValid = vehicle.DumpDate >= nowDate;
            VehicleItem.IsValid = VehicleItem.IsAuditValid && VehicleItem.IsInsuranceValid && VehicleItem.IsDumpValid;
            return Page();
        }
    }
}
