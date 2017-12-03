using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImVehicleCore.Data;
using Microsoft.AspNetCore.Authorization;
using ImVehicleCore.Interfaces;
using Microsoft.AspNetCore.Identity;
using Web.ViewModels;
using System.IO;

namespace Web.Pages.Driver
{
    public class EditModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly ITownService _townService;
        private readonly UserManager<VehicleUser> _userManager;
        private readonly IGroupService _groupService;
        public EditModel(ImVehicleCore.Data.VehicleDbContext context, IAuthorizationService authorizationService, ITownService townService, UserManager<VehicleUser> userManager,
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

        [BindProperty]
        public DriverEditViewModel DriverItem { get; set; }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> OnGetAsync(long? id, string returnUrl)
        {


            if (id == null)
            {
                return NotFound();
            }
            ReturnUrl = returnUrl;
            var driver = await _context.Drivers.SingleOrDefaultAsync(m => m.Id == id);

            if (driver == null)
            {
                return NotFound();
            }
            var townlist = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User));

            ViewData["TownList"] = new SelectList(townlist, "Id", "Name");
            if (townlist.Any())
            {
                var groups = (await _groupService.ListGroupsForTownEagerAsync(HttpContext.User, townlist.First().Id));
                ViewData["GroupList"] = new SelectList(groups, "Id", "Name");
            }

            DriverItem = new DriverEditViewModel()
            {
                Id = driver.Id,
                Name = driver.Name,
                Gender = driver.Gender,
                FirstLicenseIssueDate = driver.FirstLicenseIssueDate,
                LicenseIssue = driver.LicenseIssueDate,

                IdCardNumber = driver.IdCardNumber,
                License = driver.LicenseNumber,
                LicenseType = driver.LicenseType,
                ValidYears = driver.LicenseValidYears,
                LivingAddress = driver.LivingAddress,
                Tel = driver.Tel,
                Title = driver.Title,
                WarrantyCode = driver.WarrantyCode,

                TownId = driver.TownId,
                GroupId = driver.GroupId,

                PhotoDriverLicenseBase64 = driver.PhotoDriverLicense != null ? Convert.ToBase64String(driver.PhotoDriverLicense) : "",
                PhotoIdCard1Base64 = driver.PhotoIdCard1 != null ? Convert.ToBase64String(driver.PhotoIdCard1) : "",
                PhotoIdCard2Base64 = driver.PhotoIdCard2 != null ? Convert.ToBase64String(driver.PhotoIdCard2) : "",
                PhotoWarrantyBase64 = driver.PhotoWarranty != null ? Convert.ToBase64String(driver.PhotoWarranty) : "",

            };
            return Page();
        }

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


            MemoryStream photoWarranty = null;
            MemoryStream photoIdCard1 = null;
            MemoryStream photoIdCard2 = null;
            MemoryStream photoLicense = null;

            var acceptableExt = new[] { ".png", ".bmp", ".jpg", ".jpeg", ".tif", };

            if (acceptableExt.Contains(Path.GetExtension(DriverItem.PhotoIdCard1?.FileName)?.ToLower()))
            {
                photoIdCard1 = new MemoryStream();
                await DriverItem.PhotoIdCard1.CopyToAsync(photoIdCard1);
            }
            if (acceptableExt.Contains(Path.GetExtension(DriverItem.PhotoIdCard2?.FileName)?.ToLower()))
            {
                photoIdCard2 = new MemoryStream();
                await DriverItem.PhotoIdCard2.CopyToAsync(photoIdCard2);
            }

            if (acceptableExt.Contains(Path.GetExtension(DriverItem.PhotoWarranty?.FileName)?.ToLower()))
            {
                photoWarranty = new MemoryStream();
                await DriverItem.PhotoWarranty.CopyToAsync(photoWarranty);
            }
            if (acceptableExt.Contains(Path.GetExtension(DriverItem.PhotoDriverLicense?.FileName)?.ToLower()))
            {
                photoLicense = new MemoryStream();
                await DriverItem.PhotoDriverLicense.CopyToAsync(photoLicense);
            }


            var townId = await _userManager.IsInRoleAsync(user, "TownManager") ? user.TownId : DriverItem.TownId;

            var driver = _context.Drivers.FirstOrDefault(t => t.Id == DriverItem.Id);
            if (driver == null)
            {
                return NotFound();
            }

            driver.Name = DriverItem.Name;
            driver.Gender = DriverItem.Gender;
            driver.FirstLicenseIssueDate = DriverItem.FirstLicenseIssueDate;
            driver.LicenseIssueDate = DriverItem.LicenseIssue;

            driver.IdCardNumber = DriverItem.IdCardNumber;
            driver.LicenseNumber = DriverItem.License;
            driver.LicenseType = DriverItem.LicenseType;
            driver.LicenseValidYears = DriverItem.ValidYears;
            driver.LivingAddress = DriverItem.LivingAddress;
            driver.Tel = DriverItem.Tel;
            driver.Title = DriverItem.Title;
            driver.WarrantyCode = DriverItem.WarrantyCode;

            driver.TownId = townId;
            driver.GroupId = DriverItem.GroupId;

            if (photoLicense != null)
            {
                driver.PhotoDriverLicense = photoLicense.ToArray();
            }
            if (photoIdCard1 != null)
            {
                driver.PhotoIdCard1 = photoIdCard1.ToArray();
            }
            if (photoIdCard2 != null)
            {
                driver.PhotoIdCard2 = photoIdCard2.ToArray();
            }
            if (photoWarranty != null)
            {
                driver.PhotoWarranty = photoWarranty.ToArray();
            }
            driver.ModifyBy = user.Id;
            driver.ModificationDate = DateTime.Now;
            driver.Status = StatusType.OK;

            _context.Attach(driver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverItemExists(DriverItem.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DriverItemExists(long id)
        {
            return _context.Drivers.Any(e => e.Id == id);
        }

        public async Task<bool> IsAdmin()
        {
            var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await admin).Succeeded;
        }
    }
}
