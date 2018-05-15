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
using Socona.ImVehicle.Infrastructure.Extensions;

namespace Socona.ImVehicle.Web.Pages.Driver
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
        [BindProperty]
        public string ReturnUrl { get; set; }

        [BindProperty]
        public DriverViewModel DriverItem { get; set; }

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

            DriverItem = new DriverViewModel(driver);

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
            driver.ResidentType = DriverItem.ResidentType;

            driver.TownId = townId;
            driver.GroupId = DriverItem.GroupId;

            if (DriverItem.PhotoDriverLicense != null)
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
            if(DriverItem.PhotoAvatar!=null)
            {
                driver.PhotoAvatar =await  DriverItem.PhotoAvatar.GetPictureByteArray($"{DriverItem.Id}:{DriverItem.License}");
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
            driver.ModifyBy = user.Id;
            driver.ModificationDate = DateTime.Now;
            driver.Status = StatusType.OK;
            driver.VersionNumber += 1;
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

            return Redirect(Url.GetLocalUrl(ReturnUrl));
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
