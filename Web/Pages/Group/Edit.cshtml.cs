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

        public EditModel(ImVehicleCore.Data.VehicleDbContext context,IAuthorizationService authorizationService, UserManager<VehicleUser> userManager,     ITownService townService)
        {
            _context = context;
            _authorizationService = authorizationService;
            _userManager = userManager;
            _townService = townService;
    }

        [BindProperty]
        public GroupViewModel GroupItem { get; set; }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
         
           
            var group= await _context.Groups.SingleOrDefaultAsync(m => m.Id == id);


            if (group == null)
            {
                return NotFound();
            }
            TownList = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User))
           .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name, })
           .ToList();
            GroupItem = new GroupViewModel();
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

            var townId = await _userManager.IsInRoleAsync(user, "TownManager") ? user.TownId : GroupItem.TownId;
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
            _context.Attach(GroupItem).State = EntityState.Modified;

            await _context.SaveChangesAsync();

          
         

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

            return RedirectToPage("./Index");
        }

        private bool GroupItemExists(long id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
