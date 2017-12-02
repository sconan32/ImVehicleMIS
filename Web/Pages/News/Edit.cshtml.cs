using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImVehicleCore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.News
{
    public class EditModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;
        private readonly UserManager<VehicleUser> _userManager;
        public EditModel(ImVehicleCore.Data.VehicleDbContext context, UserManager<VehicleUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public string ReturnUrl { get; set; }

        [BindProperty]
        public NewsItem NewsItem { get; set; }

        [Authorize(Roles = "GlobalVisitor,Admins")]
        public async Task<IActionResult> OnGetAsync(long? id, string returnUrl)
        {

            if (id == null)
            {
                return NotFound();
            }
            ReturnUrl = returnUrl;
            NewsItem = await _context.Newses.SingleOrDefaultAsync(m => m.Id == id);

            if (NewsItem == null)
            {
                return NotFound();
            }
            return Page();
        }
        [Authorize(Roles = "GlobalVisitor,Admins")]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            NewsItem.ModificationDate = DateTime.Now;
            NewsItem.ModifyBy = user.Id;
            _context.Attach(NewsItem).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsItemExists(NewsItem.Id))
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

        private bool NewsItemExists(long id)
        {
            return _context.Newses.Any(e => e.Id == id);
        }
    }
}
