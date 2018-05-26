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
using Socona.ImVehicle.Infrastructure.Extensions;
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
            VehicleItem = new VehicleViewModel(vehicle);
           
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



            var vehicle = _context.Vehicles.FirstOrDefault(v => v.Id == VehicleItem.Id);
            if (vehicle == null)
            {
                return NotFound();
            }
            await VehicleItem.FillVehicleItem(vehicle);
            vehicle.FrontImage = VehicleItem.PhotoFront.UpdateUserFile(vehicle.FrontImage, _context, VisibilityType.CurrentVehicle, "车头部照片", VehicleItem.TownId, VehicleItem.GroupId);
            vehicle.RearImage = VehicleItem.PhotoRear.UpdateUserFile(vehicle.RearImage, _context, VisibilityType.CurrentVehicle, "车尾部照片", VehicleItem.TownId, VehicleItem.GroupId);
            vehicle.GpsImage = VehicleItem.PhotoGps.UpdateUserFile(vehicle.GpsImage, _context, VisibilityType.CurrentVehicle, "GPS照片", VehicleItem.TownId, VehicleItem.GroupId);
            vehicle.LicenseImage = VehicleItem.PhotoLicense.UpdateUserFile(vehicle.LicenseImage, _context, VisibilityType.CurrentVehicle, "行驶证照片", VehicleItem.TownId, VehicleItem.GroupId);
            vehicle.ExtraImage1 = VehicleItem.ExtraPhoto1.UpdateUserFile(vehicle.ExtraImage1, _context, VisibilityType.CurrentVehicle, "附加图片1", VehicleItem.TownId, VehicleItem.GroupId);
            vehicle.ExtraImage2 = VehicleItem.ExtraPhoto2.UpdateUserFile(vehicle.ExtraImage2, _context, VisibilityType.CurrentVehicle, "附加图片2", VehicleItem.TownId, VehicleItem.GroupId);
            vehicle.ExtraImage3 = VehicleItem.ExtraPhoto3.UpdateUserFile(vehicle.ExtraImage3, _context, VisibilityType.CurrentVehicle, "附加图片1", VehicleItem.TownId, VehicleItem.GroupId);

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
