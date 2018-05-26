using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Infrastructure.Extensions;
using Socona.ImVehicle.Web.ViewModels;

namespace Socona.ImVehicle.Web.Pages.Vehicle
{
    public class CreateModel : PageModel
    {
        private readonly Socona.ImVehicle.Core.Data.VehicleDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly ITownService _townService;
        private readonly UserManager<VehicleUser> _userManager;
        private readonly IGroupService _groupService;
        public CreateModel(Socona.ImVehicle.Core.Data.VehicleDbContext context, IAuthorizationService authorizationService, ITownService townService, UserManager<VehicleUser> userManager,
            IGroupService groupService)
        {
            _context = context;
            _authorizationService = authorizationService;
            _townService = townService;
            _userManager = userManager;
            _groupService = groupService;
  
        }

        [BindProperty]
        public string ReturnUrl { get; set; }


        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> OnGetAsync(long? groupId, string returnUrl)
        {
            ReturnUrl = returnUrl;

            var townlist = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User));

            ViewData["TownList"] = new SelectList(townlist, "Id", "Name");
           
            VehicleItem = new VehicleViewModel();
            VehicleItem.GroupId = groupId;
            if (groupId != null)
            {
                VehicleItem.TownId = townlist.FirstOrDefault(t => t.Groups.Any(u => u.Id == groupId))?.Id;
            }
            else
            {
                VehicleItem.TownId = townlist.FirstOrDefault()?.Id;
            }
            if (townlist.Any())
            {
                var groups = (await _groupService.ListGroupsForTownEagerAsync(HttpContext.User, VehicleItem.TownId??-1));
                ViewData["GroupList"] = new SelectList(groups, "Id", "Name");
            }
            return Page();
        }

        [BindProperty(SupportsGet =true)]
        public VehicleViewModel VehicleItem { get; set; }


        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var townlist = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User));

                ViewData["TownList"] = new SelectList(townlist, "Id", "Name");           
                         
              
                if (townlist.Any())
                {
                    var groups = (await _groupService.ListGroupsForTownEagerAsync(HttpContext.User, VehicleItem.TownId??-1));
                    ViewData["GroupList"] = new SelectList(groups, "Id", "Name");
                }

                return Page();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

         
            var vehicle = new VehicleItem()
            {
                Id = VehicleItem.Id,
                Name = VehicleItem.Name,
                Brand = VehicleItem.Brand,
                Color = VehicleItem.Color,
                Comment = VehicleItem.Comment,
                AuditExpiredDate = VehicleItem.AuditExpiredDate,
                DumpDate = VehicleItem.DumpDate,
                LicenceNumber = VehicleItem.License,
                ProductionDate = VehicleItem.ProductionDate,
                RealOwner = VehicleItem.RealOwner,

                Type = VehicleItem.Type,
                Usage = VehicleItem.Usage,
                Agent = VehicleItem.Agent,
                DriverId = VehicleItem.DriverId,
                GpsEnabled = VehicleItem.GpsEnabled,
                FirstRegisterDate = VehicleItem.RegisterDate,

                YearlyAuditDate = VehicleItem.YearlyAuditDate,
                VehicleStatus = VehicleItem.VehicleStatus,

                GroupId = VehicleItem.GroupId,
                TownId = VehicleItem.TownId,



                CreateBy = user.Id,
                CreationDate = DateTime.Now,
                Status = StatusType.OK,
            };


            vehicle.FrontImage = VehicleItem.PhotoFront.UpdateUserFile(vehicle.FrontImage, _context, VisibilityType.CurrentVehicle, "车头部照片", VehicleItem.TownId, VehicleItem.GroupId);
            vehicle.RearImage = VehicleItem.PhotoRear.UpdateUserFile(vehicle.RearImage, _context, VisibilityType.CurrentVehicle, "车尾部照片", VehicleItem.TownId, VehicleItem.GroupId);
            vehicle.GpsImage = VehicleItem.PhotoGps.UpdateUserFile(vehicle.GpsImage, _context, VisibilityType.CurrentVehicle, "GPS照片", VehicleItem.TownId, VehicleItem.GroupId);
            vehicle.LicenseImage = VehicleItem.PhotoLicense.UpdateUserFile(vehicle.LicenseImage, _context, VisibilityType.CurrentVehicle, "行驶证照片", VehicleItem.TownId, VehicleItem.GroupId);
            vehicle.ExtraImage1 = VehicleItem.ExtraPhoto1.UpdateUserFile(vehicle.ExtraImage1, _context, VisibilityType.CurrentVehicle, "附加图片1", VehicleItem.TownId, VehicleItem.GroupId);
            vehicle.ExtraImage2 = VehicleItem.ExtraPhoto2.UpdateUserFile(vehicle.ExtraImage2, _context, VisibilityType.CurrentVehicle, "附加图片2", VehicleItem.TownId, VehicleItem.GroupId);
            vehicle.ExtraImage3 = VehicleItem.ExtraPhoto3.UpdateUserFile(vehicle.ExtraImage3, _context, VisibilityType.CurrentVehicle, "附加图片1", VehicleItem.TownId, VehicleItem.GroupId);
            _context.Vehicles.Add(vehicle);
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