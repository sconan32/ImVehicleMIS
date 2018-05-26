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

            driver.IdCardImage1 = DriverItem.PhotoIdCard1.UpdateUserFile(driver.IdCardImage1, _context, VisibilityType.CurrentDriver, "身份证国徽面图片", DriverItem.TownId, DriverItem.GroupId);
            driver.IdCardImage2 = DriverItem.PhotoIdCard2.UpdateUserFile(driver.IdCardImage2, _context, VisibilityType.CurrentDriver, "身份证相片面图片", DriverItem.TownId, DriverItem.GroupId);
            driver.LicenseImage = DriverItem.PhotoDriverLicense.UpdateUserFile(driver.LicenseImage, _context, VisibilityType.CurrentDriver, "驾驶证照片", DriverItem.TownId, DriverItem.GroupId);
            driver.AvatarImage = DriverItem.PhotoAvatar.UpdateUserFile(driver.AvatarImage, _context, VisibilityType.CurrentDriver, "驾驶员图片", DriverItem.TownId, DriverItem.GroupId);
            driver.ExtraImage1 = DriverItem.ExtraImage1.UpdateUserFile(driver.ExtraImage1, _context, VisibilityType.CurrentDriver, "附加图片1", DriverItem.TownId, DriverItem.GroupId);
            driver.ExtraImage2 = DriverItem.ExtraImage2.UpdateUserFile(driver.ExtraImage2, _context, VisibilityType.CurrentDriver, "附加图片2", DriverItem.TownId, DriverItem.GroupId);
            driver.ExtraImage3 = DriverItem.ExtraImage3.UpdateUserFile(driver.ExtraImage3, _context, VisibilityType.CurrentDriver, "附加图片3", DriverItem.TownId, DriverItem.GroupId);

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
