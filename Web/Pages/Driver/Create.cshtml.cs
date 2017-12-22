using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Socona.ImVehicle.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Socona.ImVehicle.Web.ViewModels;
using Socona.ImVehicle.Core.Interfaces;
using System.IO;
using Microsoft.AspNetCore.Identity;

namespace Web.Pages.Driver
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


            MemoryStream photoWarranty = new MemoryStream();
            MemoryStream photoIdCard1 = new MemoryStream();
            MemoryStream photoIdCard2 = new MemoryStream();
            MemoryStream photoLicense = new MemoryStream();

            var acceptableExt = new[] { ".png", ".bmp", ".jpg", ".jpeg", ".tif", };

            if (acceptableExt.Contains(Path.GetExtension(DriverItem.PhotoIdCard1?.FileName)?.ToLower()))
            {
                await DriverItem.PhotoIdCard1.CopyToAsync(photoIdCard1);
            }
            if (acceptableExt.Contains(Path.GetExtension(DriverItem.PhotoIdCard2?.FileName)?.ToLower()))
            {
                await DriverItem.PhotoIdCard2.CopyToAsync(photoIdCard2);
            }

            if (acceptableExt.Contains(Path.GetExtension(DriverItem.PhotoWarranty?.FileName)?.ToLower()))
            {
                await DriverItem.PhotoWarranty.CopyToAsync(photoWarranty);
            }
            if (acceptableExt.Contains(Path.GetExtension(DriverItem.PhotoDriverLicense?.FileName)?.ToLower()))
            {
                await DriverItem.PhotoDriverLicense.CopyToAsync(photoIdCard1);
            }


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

                PhotoDriverLicense = photoLicense.ToArray(),
                PhotoIdCard1 = photoIdCard1.ToArray(),
                PhotoIdCard2 = photoIdCard2.ToArray(),
                PhotoWarranty = photoWarranty.ToArray(),

                CreateBy = user.Id,
                CreationDate = DateTime.Now,
                Status = StatusType.OK,
            };
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