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

namespace Socona.ImVehicle.Web.Pages.Driver
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
            DriverItem = new DriverEditViewModel();
            ReturnUrl = returnUrl;

            var townlist = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User));

            ViewData["TownList"] = new SelectList(townlist, "Id", "Name");
            if (townlist.Any())
            {
                var groups = (await _groupService.ListGroupsForTownEagerAsync(HttpContext.User, townlist.First().Id));
                ViewData["GroupList"] = new SelectList(groups, "Id", "Name");
            }

            DriverItem.GroupId = groupId;
            if (groupId != null)
            {
                DriverItem.TownId = townlist.FirstOrDefault(t => t.Groups.Any(u => u.Id == groupId))?.Id;
            }

            return Page();
        }

        [BindProperty(SupportsGet = true)]
        public DriverEditViewModel DriverItem { get; set; }
        [Authorize(Roles = "TownManager,Admins")]
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


            var townId = await _userManager.IsInRoleAsync(user, "TownManager") ? user.TownId : DriverItem.TownId;
            var driver = new DriverItem
            {
                Name = DriverItem.Name,
                Gender = DriverItem.Gender,
                FirstLicenseIssueDate = DriverItem.FirstLicenseIssueDate,
                LicenseIssueDate = DriverItem.LicenseIssue,

                IdCardNumber = DriverItem.IdCardNumber,
                LicenseNumber = DriverItem.License,
                LicenseType = DriverItem.LicenseType,
                LicenseValidYears = DriverItem.ValidYears,
                LivingAddress = DriverItem.LivingAddress,
                Tel = DriverItem.Tel,
                Title = DriverItem.Title,
                WarrantyCode = DriverItem.WarrantyCode,
                ResidentType = DriverItem.ResidentType,
                TownId = townId,
                GroupId = DriverItem.GroupId,


                CreateBy = user.Id,
                CreationDate = DateTime.Now,
                Status = StatusType.OK,
            };
            if(DriverItem.PhotoDriverLicense!=null)
            {
                driver.PhotoDriverLicense = await DriverItem.PhotoDriverLicense.GetPictureByteArray($"{DriverItem.Id}:{DriverItem.License}");
            }
            if (DriverItem.PhotoIdCard1 != null)
            {
                driver.PhotoIdCard1 = await DriverItem.PhotoIdCard1.GetPictureByteArray($"{DriverItem.Id}:{DriverItem.License}");
            }
            if (DriverItem.PhotoIdCard2 != null)
            {
                driver.PhotoIdCard2 = await DriverItem.PhotoIdCard2.GetPictureByteArray($"{DriverItem.Id}:{DriverItem.License}");
            }
            if (DriverItem.PhotoWarranty != null)
            {
                driver.PhotoWarranty = await DriverItem.PhotoWarranty.GetPictureByteArray($"{DriverItem.Id}:{DriverItem.License}");
            }

            if (DriverItem.ExtraPhoto1 != null)
            {
                driver.ExtraPhoto1 = await DriverItem.ExtraPhoto1.GetPictureByteArray($"{DriverItem.Id}:{DriverItem.License}");
            }
            if (DriverItem.ExtraPhoto2 != null)
            {
                driver.ExtraPhoto2 = await DriverItem.ExtraPhoto2.GetPictureByteArray($"{DriverItem.Id}:{DriverItem.License}");
            }
            if (DriverItem.ExtraPhoto3 != null)
            {
                driver.ExtraPhoto3 = await DriverItem.ExtraPhoto3.GetPictureByteArray($"{DriverItem.Id}:{DriverItem.License}");
            }

            _context.Drivers.Add(driver);
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