using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Web.ViewModels;

namespace Socona.ImVehicle.Web.Pages.News
{
    public class EditModel : PageModel
    {
        private readonly VehicleDbContext _context;
        private readonly UserManager<VehicleUser> _userManager;
        public EditModel(Socona.ImVehicle.Core.Data.VehicleDbContext context, UserManager<VehicleUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public string ReturnUrl { get; set; }

        [BindProperty]
        public NewsEditViewModel NewsItem { get; set; }

        [Authorize(Roles = "GlobalVisitor,Admins")]
        public async Task<IActionResult> OnGetAsync(long? id, string returnUrl)
        {

            if (id == null)
            {
                return NotFound();
            }
            ReturnUrl = returnUrl;
            var news = await _context.Newses.SingleOrDefaultAsync(m => m.Id == id);


            if (news == null)
            {
                return NotFound();
            }

            NewsItem = new NewsEditViewModel(news);
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


            var news = _context.Newses.FirstOrDefault(v => v.Id == NewsItem.Id);
            if (news == null)
            {
                return NotFound();
            }
            await NewsItem.FillNewsItem(news);

            news.ModificationDate = DateTime.Now;
            news.ModifyBy = user.Id;
            news.VersionNumber += 1;
            _context.Attach(news).State = EntityState.Modified;


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
