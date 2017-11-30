using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImVehicleCore.Data;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ImVehicleCore.Interfaces;
using Web.ViewModels;

namespace Web.Pages.Group
{
    public class CreateModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;
        private readonly UserManager<VehicleUser> _userManager;
        private readonly ITownService _townService;
        private readonly IAuthorizationService _authorizationService;

        public CreateModel(ImVehicleCore.Data.VehicleDbContext context, UserManager<VehicleUser> signInManager, IAuthorizationService authorizationService, ITownService townService)
        {
            _context = context;
            _userManager = signInManager;
            _authorizationService = authorizationService;
            _townService = townService;
        }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> OnGetAsync()
        {

            TownList = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User))
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name, })
                .ToList();
            return Page();

        }
        public List<SelectListItem> TownList { get; set; }

        [BindProperty]
        public GroupEditViewModel GroupItem { get; set; }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);


            MemoryStream warPhotoS = new MemoryStream();
            MemoryStream secPhotoS = new MemoryStream();


            MemoryStream mainPhotoS = new MemoryStream();
            var acceptableExt = new[] { ".png", ".bmp", ".jpg", ".jpeg", ".tif", };
            if (acceptableExt.Contains(Path.GetExtension(GroupItem.PhotoMain?.FileName)?.ToLower()))
            {
                await GroupItem.PhotoMain.CopyToAsync(mainPhotoS);
            }

            if (acceptableExt.Contains(Path.GetExtension(GroupItem.PhotoSecurity?.FileName)?.ToLower()))
            {
                await GroupItem.PhotoSecurity.CopyToAsync(secPhotoS);
            }
            if (acceptableExt.Contains(Path.GetExtension(GroupItem.PhotoWarranty?.FileName)?.ToLower()))
            {
                await GroupItem.PhotoWarranty.CopyToAsync(warPhotoS);
            }

            var townId = await _userManager.IsInRoleAsync(user,"TownManager") ? user.TownId : GroupItem.TownId;
            var group = new GroupItem()
            {

                Name = GroupItem.Name,
                Address = GroupItem.Address,
                RegisterAddress = GroupItem.RegisterAddress,
                License = GroupItem.License,
                ChiefName = GroupItem.ChiefName,
                ChiefTel = GroupItem.ChiefTel,
                Type = GroupItem.Type,
                TownId = townId,
                PhotoMain = mainPhotoS.ToArray(),
                PhotoWarranty = warPhotoS.ToArray(),
                PhotoSecurity = warPhotoS.ToArray(),
                Code = GroupItem.Code,
                ChiefTitle = GroupItem.ChiefTitle,
                Comment = GroupItem.Comment,


                CreationDate = DateTime.Now,
                CreateBy = user.Id,
                Status = StatusType.OK,
            };

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        public async Task<bool> IsAdmin()
        {
            var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await admin).Succeeded;
        }
      


    }
}