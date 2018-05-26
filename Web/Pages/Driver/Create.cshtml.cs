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
            DriverItem = new DriverViewModel();
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
        public DriverViewModel DriverItem { get; set; }
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


            DriverItem.TownId = await _userManager.IsInRoleAsync(user, "TownManager") ? user.TownId : DriverItem.TownId;
            var driver = new DriverItem();
            await DriverItem.FillDriverItemAsync(driver);

            driver.CreateBy = user.Id;
            driver.CreationDate = DateTime.Now;
            driver.Status = StatusType.OK;
            driver.IdCardImage1 = DriverItem.PhotoIdCard1.UpdateUserFile(driver.IdCardImage1, _context,VisibilityType.CurrentDriver, "身份证国徽面图片", DriverItem.TownId, DriverItem.GroupId);
            driver.IdCardImage2 = DriverItem.PhotoIdCard2.UpdateUserFile(driver.IdCardImage2, _context, VisibilityType.CurrentDriver, "身份证相片面图片", DriverItem.TownId, DriverItem.GroupId);
            driver.LicenseImage = DriverItem.PhotoDriverLicense.UpdateUserFile(driver.LicenseImage, _context, VisibilityType.CurrentDriver, "驾驶证照片", DriverItem.TownId, DriverItem.GroupId);
            driver.AvatarImage = DriverItem.PhotoAvatar.UpdateUserFile(driver.AvatarImage, _context, VisibilityType.CurrentDriver, "驾驶员图片", DriverItem.TownId, DriverItem.GroupId);
            driver.ExtraImage1 = DriverItem.ExtraImage1.UpdateUserFile(driver.ExtraImage1, _context, VisibilityType.CurrentDriver, "附加图片1", DriverItem.TownId, DriverItem.GroupId);
            driver.ExtraImage2 = DriverItem.ExtraImage2.UpdateUserFile(driver.ExtraImage2, _context, VisibilityType.CurrentDriver, "附加图片2", DriverItem.TownId, DriverItem.GroupId);
            driver.ExtraImage3 = DriverItem.ExtraImage3.UpdateUserFile(driver.ExtraImage3, _context, VisibilityType.CurrentDriver, "附加图片3", DriverItem.TownId, DriverItem.GroupId);

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