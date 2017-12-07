using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Socona.ImVehicle.Core.Interfaces;
using Web.ViewModels;

namespace Web.Pages.Vehicle
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
                .SingleOrDefaultAsync(m => m.Id == id);

            if (vehicle == null)
            {
                return NotFound();
            }


            VehicleItem = new VehicleViewModel()
            {
                Id = vehicle.Id,
                Brand = vehicle.Brand,
                Color = vehicle.Color,
                Comment = vehicle.Comment,
                DriverId = vehicle.DriverId,
                GroupId = vehicle.GroupId,
                InsuranceExpiredDate = vehicle.InsuranceExpiredDate,
                LastRegisterDate = vehicle.LastRegisterDate,
                RegisterDate = vehicle.LastRegisterDate,
                License = vehicle.LicenceNumber,
                Name = vehicle.Name,
                ProductionDate = vehicle.ProductionDate,
                RealOwner = vehicle.RealOwner,
                Type = vehicle.Type,
                Usage = vehicle.Usage,
                VehicleStatus = vehicle.VehicleStatus,
                YearlyAuditDate = vehicle.YearlyAuditDate,
                Agent = vehicle.Agent,
                DumpDate = vehicle.DumpDate,
                GroupName = vehicle.Group?.Name,
                TownName = vehicle.Town?.Name,
                DriverName = vehicle.Driver?.Name,
                DriverTel = vehicle.Driver?.Tel,

                PhotoLicenseBase64 = vehicle.PhotoLicense != null ? Convert.ToBase64String(vehicle.PhotoLicense) : "",
                PhotoGpsBase64 = vehicle.PhotoGps != null ? Convert.ToBase64String(vehicle.PhotoGps) : "",
                PhotoAuditBase64 = vehicle.PhotoAudit != null ? Convert.ToBase64String(vehicle.PhotoAudit) : "",
                PhotoFrontBase64 = vehicle.PhotoFront != null ? Convert.ToBase64String(vehicle.PhotoFront) : "",
                PhotoRearBase64 = vehicle.PhotoRear != null ? Convert.ToBase64String(vehicle.PhotoRear) : "",
                PhotoInsuaranceBase64 = vehicle.PhotoInsuarance != null ? Convert.ToBase64String(vehicle.PhotoInsuarance) : "",




            };
            var nowDate = DateTime.Now.Date;
            VehicleItem.IsAuditValid = VehicleItem.YearlyAuditDate?.AddYears(1) >= nowDate;
            VehicleItem.IsInsuranceValid = vehicle.InsuranceExpiredDate?.AddYears(1) >= nowDate;
            VehicleItem.IsDumpValid = vehicle.DumpDate >= nowDate;
            VehicleItem.IsValid = VehicleItem.IsAuditValid && VehicleItem.IsInsuranceValid && VehicleItem.IsDumpValid;
            return Page();
        }
    }
}
