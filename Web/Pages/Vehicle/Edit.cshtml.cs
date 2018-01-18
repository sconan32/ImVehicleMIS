using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Web.ViewModels;

namespace Socona.ImVehicle.Web.Pages.Vehicle
{
    public class EditModel : PageModel
    {
        private readonly VehicleDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly ITownService _townService;
        private readonly UserManager<VehicleUser> _userManager;
        private readonly IGroupService _groupService;
        public EditModel(VehicleDbContext context, IAuthorizationService authorizationService, ITownService townService, UserManager<VehicleUser> userManager,
            IGroupService groupService)
        {
            _context = context;
            _authorizationService = authorizationService;
            _townService = townService;
            _userManager = userManager;
            _groupService = groupService;

        }

        public async Task<IActionResult> OnGetAsync(long? id, string returnUrl)
        {
            ReturnUrl = returnUrl;
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(t => t.Id == id);
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
                DumpDate = vehicle.DumpDate,
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
                TownId = vehicle.TownId,
                Agent = vehicle.Agent,
                GpsEnabled = vehicle.GpsEnabled ?? false,


                PhotoAuditBase64 = vehicle.PhotoAudit != null ? Convert.ToBase64String(vehicle.PhotoAudit) : "",
                PhotoFrontBase64 = vehicle.PhotoFront != null ? Convert.ToBase64String(vehicle.PhotoFront) : "",
                PhotoRearBase64 = vehicle.PhotoRear != null ? Convert.ToBase64String(vehicle.PhotoRear) : "",
                PhotoInsuaranceBase64 = vehicle.PhotoInsuarance != null ? Convert.ToBase64String(vehicle.PhotoInsuarance) : "",
                PhotoGpsBase64 = vehicle.PhotoGps != null ? Convert.ToBase64String(vehicle.PhotoGps) : "",
                PhotoLicenseBase64 = vehicle.PhotoGps != null ? Convert.ToBase64String(vehicle.PhotoLicense) : "",
            };
            var townlist = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User));
            ViewData["TownList"] = new SelectList(townlist, "Id", "Name");
            if (townlist.Any())
            {
                var groups = (await _groupService.ListGroupsForTownEagerAsync(HttpContext.User, townlist.First().Id));
                ViewData["GroupList"] = new SelectList(groups, "Id", "Name");
            }
            
            if (VehicleItem.GroupId != null)
            {
                VehicleItem.TownId = townlist.FirstOrDefault(t => t.Groups.Any(u => u.Id == VehicleItem.GroupId))?.Id;
            }
            if (VehicleItem.DriverId != null)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var drivers = await (_context.Drivers.Where(t => t.TownId == user.TownId)).ToListAsync();
                ViewData["DriverList"] = new SelectList(drivers, "Id", "Name");
            }
            return Page();
        }

        [BindProperty]
        public VehicleViewModel VehicleItem { get; set; }
        [BindProperty]
        public string ReturnUrl { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var townlist = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User));

                ViewData["TownList"] = new SelectList(townlist, "Id", "Name");
                if (townlist.Any())
                {
                    var groups = (await _groupService.ListGroupsForTownEagerAsync(HttpContext.User, townlist.First().Id));
                    ViewData["GroupList"] = new SelectList(groups, "Id", "Name");
                }

                return Page();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            MemoryStream spFront = null;
            MemoryStream spRear = null;
            MemoryStream spAudit = null;
            MemoryStream spInsuarance = null;
            MemoryStream spGps = null;
            MemoryStream spLicense = null;
            var acceptableExt = new[] { ".png", ".bmp", ".jpg", ".jpeg", ".tif", };

            if (acceptableExt.Contains(Path.GetExtension(VehicleItem.PhotoRear?.FileName)?.ToLower()))
            {
                spRear = new MemoryStream();
                await VehicleItem.PhotoRear.CopyToAsync(spRear);
            }
            if (acceptableExt.Contains(Path.GetExtension(VehicleItem.PhotoFront?.FileName)?.ToLower()))
            {
                spFront = new MemoryStream();
                await VehicleItem.PhotoFront.CopyToAsync(spFront);
            }

            if (acceptableExt.Contains(Path.GetExtension(VehicleItem.PhotoAudit?.FileName)?.ToLower()))
            {
                spAudit = new MemoryStream();
                await VehicleItem.PhotoAudit.CopyToAsync(spAudit);
            }
            if (acceptableExt.Contains(Path.GetExtension(VehicleItem.PhotoInsuarance?.FileName)?.ToLower()))
            {
                spInsuarance = new MemoryStream();
                await VehicleItem.PhotoInsuarance.CopyToAsync(spInsuarance);
            }
            if (acceptableExt.Contains(Path.GetExtension(VehicleItem.PhotoGps?.FileName)?.ToLower()))
            {
                spGps = new MemoryStream();
                await VehicleItem.PhotoGps.CopyToAsync(spGps);
            }
            if (acceptableExt.Contains(Path.GetExtension(VehicleItem.PhotoLicense?.FileName)?.ToLower()))
            {
                spLicense = new MemoryStream();
                await VehicleItem.PhotoLicense.CopyToAsync(spLicense);
            }

            var vehicle = _context.Vehicles.FirstOrDefault(v => v.Id == VehicleItem.Id);
            if (vehicle == null)
            {
                return NotFound();
            }

           
            vehicle.Name = VehicleItem.Name;
            vehicle.Brand = VehicleItem.Brand;
            vehicle.Color = VehicleItem.Color;
            vehicle.Comment = VehicleItem.Comment;
            vehicle.InsuranceExpiredDate = VehicleItem.InsuranceExpiredDate;
            vehicle.LicenceNumber = VehicleItem.License;
            vehicle.DumpDate = VehicleItem.DumpDate;
            vehicle.ProductionDate = VehicleItem.ProductionDate;
            vehicle.RealOwner = VehicleItem.RealOwner;
            vehicle.LastRegisterDate = VehicleItem.RegisterDate;
            vehicle.Type = VehicleItem.Type;
            vehicle.Usage = VehicleItem.Usage;
            vehicle.YearlyAuditDate = VehicleItem.YearlyAuditDate;
            vehicle.VehicleStatus = VehicleItem.VehicleStatus;
            vehicle.GroupId = VehicleItem.GroupId;
            vehicle.TownId = VehicleItem.TownId;
            vehicle.DriverId = VehicleItem.DriverId;
            vehicle.GpsEnabled = VehicleItem.GpsEnabled;
            vehicle.Agent = VehicleItem.Agent;
            if (spFront != null)
            {
                vehicle.PhotoFront = spFront.ToArray();
            }
            if (spRear != null)
            {
                vehicle.PhotoRear = spRear.ToArray();
            }
            if (spAudit != null)
            {
                vehicle.PhotoAudit = spAudit.ToArray();
            }
            if (spInsuarance != null)
            {
                vehicle.PhotoInsuarance = spInsuarance.ToArray();
            }
            if (spGps != null)
            {
                vehicle.PhotoGps = spGps.ToArray();
            }
            if (spLicense != null)
            {
                vehicle.PhotoLicense = spLicense.ToArray();
            }
            vehicle.ModifyBy  = user.Id;
            vehicle.ModificationDate  = DateTime.Now;
            vehicle.Status = StatusType.OK;
            vehicle.VersionNumber += 1;


            _context.Entry (vehicle).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Redirect(Url.GetLocalUrl(ReturnUrl));
        }

        public async Task<bool> IsAdmin()
        {
            var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await admin).Succeeded;
        }
    }
}
