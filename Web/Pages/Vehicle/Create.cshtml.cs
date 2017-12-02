using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImVehicleCore.Data;
using Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using ImVehicleCore.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.IO;

namespace Web.Pages.Vehicle
{
    public class CreateModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly ITownService _townService;
        private readonly UserManager<VehicleUser> _userManager;
        private readonly IGroupService _groupService;
        public CreateModel(ImVehicleCore.Data.VehicleDbContext context, IAuthorizationService authorizationService, ITownService townService, UserManager<VehicleUser> userManager,
            IGroupService groupService)
        {
            _context = context;
            _authorizationService = authorizationService;
            _townService = townService;
            _userManager = userManager;
            _groupService = groupService;
            VehicleItem = new VehicleEditViewModel();
        }

        [BindProperty]
        public string ReturnUrl { get; set; }
        public async Task<IActionResult> OnGetAsync(long? groupId, string returnUrl)
        {
            ReturnUrl = returnUrl;

            var townlist = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User));

            ViewData["TownList"] = new SelectList(townlist, "Id", "Name");
            if (townlist.Any())
            {
                var groups = (await _groupService.ListGroupsForTownEagerAsync(HttpContext.User, townlist.First().Id));
                ViewData["GroupList"] = new SelectList(groups, "Id", "Name");
            }

            VehicleItem.GroupId = groupId;
            if (groupId != null)
            {
                VehicleItem.TownId = townlist.FirstOrDefault(t => t.Groups.Any(u => u.Id == groupId))?.Id;
            }
            return Page();
        }

        [BindProperty]
        public VehicleEditViewModel VehicleItem { get; set; }

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

            MemoryStream spFront = new MemoryStream();
            MemoryStream spRear = new MemoryStream();
            MemoryStream spAudit = new MemoryStream();
            MemoryStream spInsuarance = new MemoryStream();

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



            var vehicle = new VehicleItem()
            {
                Id = VehicleItem.Id,
                Name = VehicleItem.Name,
                Brand = VehicleItem.Brand,
                Color = VehicleItem.Color,
                Comment = VehicleItem.Comment,
                InsuranceExpiredDate = VehicleItem.InsuranceExpiredDate,
                LicenceNumber = VehicleItem.License,
                ProductionDate = VehicleItem.ProductionDate,
                RealOwner = VehicleItem.RealOwner,
                RegisterDate = VehicleItem.RegisterDate,
                Type = VehicleItem.Type,
                Usage = VehicleItem.Usage,
                YearlyAuditDate = VehicleItem.YearlyAuditDate,
                VehicleStatus = VehicleItem.VehicleStatus,



                PhotoFront = spFront.ToArray(),
                PhotoRear = spRear.ToArray(),
                PhotoAudit = spAudit.ToArray(),
                PhotoInsuarance = spInsuarance.ToArray(),


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