using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImVehicleCore.Data;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Web.ViewModels;
using ImVehicleCore.Interfaces;

namespace Web.Pages.Group
{
    public class EditModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<VehicleUser> _userManager;
        private readonly ITownService _townService;

        public EditModel(ImVehicleCore.Data.VehicleDbContext context, IAuthorizationService authorizationService, UserManager<VehicleUser> userManager, ITownService townService)
        {
            _context = context;
            _authorizationService = authorizationService;
            _userManager = userManager;
            _townService = townService;
        }
        [BindProperty]
        public string ReturnUrl { get; set; }
        [BindProperty]
        public GroupEditViewModel GroupItem { get; set; }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> OnGetAsync(long? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            ReturnUrl = returnUrl;
            var group = await _context.Groups.SingleOrDefaultAsync(m => m.Id == id);


            if (group == null)
            {
                return NotFound();
            }
            TownList = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User))
           .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name, })
           .ToList();

            GroupItem = new GroupEditViewModel()
            {
                Id = group.Id,
                Name = group.Name,
                Address = group.Address,
                ChiefName = group.ChiefName,
                ChiefTel = group.ChiefTel,
                ChiefTitle = group.ChiefTitle,
                Code = group.Code,
                Comment = group.Comment,
                License = group.License,

                RegisterAddress = group.RegisterAddress,
                TownId = group.TownId ?? 0,
                Type = group.Type,
                PhotoMainBase64 = group.PhotoMain != null ? Convert.ToBase64String(group.PhotoMain) : "",
                PhotoSecurityBase64 = group.PhotoSecurity != null ? Convert.ToBase64String(group.PhotoSecurity) : "",
                PhotoWarrantyBase64 = group.PhotoWarranty != null ? Convert.ToBase64String(group.PhotoWarranty) : "",
            };
            return Page();
        }

        public List<SelectListItem> TownList { get; set; }

        public async Task<bool> IsAdmin()
        {
            var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await admin).Succeeded;
        }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);


            MemoryStream warPhotoS = null;
            MemoryStream secPhotoS = null;


            MemoryStream mainPhotoS = null;

            var acceptableExt = new[] { ".png", ".bmp", ".jpg", ".jpeg", ".tif", };
            if (acceptableExt.Contains(Path.GetExtension(GroupItem.PhotoMain?.FileName)?.ToLower()))
            {

                mainPhotoS = new MemoryStream();
                await GroupItem.PhotoMain.CopyToAsync(mainPhotoS);
            }

            if (acceptableExt.Contains(Path.GetExtension(GroupItem.PhotoSecurity?.FileName)?.ToLower()))
            {
                secPhotoS = new MemoryStream();
                await GroupItem.PhotoSecurity.CopyToAsync(secPhotoS);
            }
            if (acceptableExt.Contains(Path.GetExtension(GroupItem.PhotoWarranty?.FileName)?.ToLower()))
            {
                warPhotoS = new MemoryStream();
                await GroupItem.PhotoWarranty.CopyToAsync(warPhotoS);
            }



            var townId = await _userManager.IsInRoleAsync(user, "TownManager") ? user.TownId : GroupItem.TownId;
            var group = _context.Groups.FirstOrDefault(t => t.Id == GroupItem.Id);
            if (group == null)
            {
                return NotFound();
            }



            group.Name = GroupItem.Name;
            group.Address = GroupItem.Address;
            group.RegisterAddress = GroupItem.RegisterAddress;
            group.License = GroupItem.License;
            group.ChiefName = GroupItem.ChiefName;
            group.ChiefTel = GroupItem.ChiefTel;
            group.Type = GroupItem.Type;
            group.TownId = townId;
            group.PhotoMain = mainPhotoS?.ToArray() ?? group.PhotoMain;
            group.PhotoWarranty = warPhotoS?.ToArray() ?? group.PhotoWarranty;
            group.PhotoSecurity = secPhotoS?.ToArray() ?? group.PhotoSecurity;
            group.Code = GroupItem.Code;
            group.ChiefTitle = GroupItem.ChiefTitle;
            group.Comment = GroupItem.Comment;


            group.ModificationDate = DateTime.Now;
            group.ModifyBy = user.Id;
            group.Status = StatusType.OK;


            _context.Attach(group).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupItemExists(GroupItem.Id))
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

        private bool GroupItemExists(long id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
