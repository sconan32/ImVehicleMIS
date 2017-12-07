using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Socona.ImVehicle.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.IO;

namespace Web.Pages.Vehicle
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

            MemoryStream spFront = new MemoryStream();
            MemoryStream spRear = new MemoryStream();
            MemoryStream spAudit = new MemoryStream();
            MemoryStream spInsuarance = new MemoryStream();
            MemoryStream spGps = new MemoryStream();
            MemoryStream spLicense = new MemoryStream();

            var acceptableExt = new[] { ".png", ".bmp", ".jpg", ".jpeg", ".tif", };

            if (acceptableExt.Contains(Path.GetExtension(VehicleItem.PhotoRear?.FileName)?.ToLower()))
            {
                await VehicleItem.PhotoRear.CopyToAsync(spRear);
            }
            if (acceptableExt.Contains(Path.GetExtension(VehicleItem.PhotoFront?.FileName)?.ToLower()))
            {
                await VehicleItem.PhotoFront.CopyToAsync(spFront);
            }

            if (acceptableExt.Contains(Path.GetExtension(VehicleItem.PhotoAudit?.FileName)?.ToLower()))
            {
                await VehicleItem.PhotoAudit.CopyToAsync(spAudit);
            }
            if (acceptableExt.Contains(Path.GetExtension(VehicleItem.PhotoInsuarance?.FileName)?.ToLower()))
            {
                await VehicleItem.PhotoInsuarance.CopyToAsync(spInsuarance);
            }
            if (acceptableExt.Contains(Path.GetExtension(VehicleItem.PhotoGps?.FileName)?.ToLower()))
            {
                await VehicleItem.PhotoGps.CopyToAsync(spGps);
            }
            if (acceptableExt.Contains(Path.GetExtension(VehicleItem.PhotoLicense?.FileName)?.ToLower()))
            {
                await VehicleItem.PhotoLicense.CopyToAsync(spLicense);
            }

            var vehicle = new VehicleItem()
            {
                Id = VehicleItem.Id,
                Name = VehicleItem.Name,
                Brand = VehicleItem.Brand,
                Color = VehicleItem.Color,
                Comment = VehicleItem.Comment,
                InsuranceExpiredDate = VehicleItem.InsuranceExpiredDate,
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

                PhotoFront = spFront.ToArray(),
                PhotoRear = spRear.ToArray(),
                PhotoAudit = spAudit.ToArray(),
                PhotoInsuarance = spInsuarance.ToArray(),
                PhotoGps = spGps.ToArray(),
                PhotoLicense = spLicense.ToArray(),

                CreateBy = user.Id,
                CreationDate = DateTime.Now,
                Status = StatusType.OK,
            };


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