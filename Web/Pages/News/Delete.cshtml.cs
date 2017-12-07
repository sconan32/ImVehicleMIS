using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.News
{
    public class DeleteModel : PageModel
    {
        private readonly Socona.ImVehicle.Core.Data.VehicleDbContext _context;

        public DeleteModel(Socona.ImVehicle.Core.Data.VehicleDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public NewsItem NewsItem { get; set; }
        [BindProperty]
        public string ReturnUrl { get; set; }

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
        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NewsItem = await _context.Newses.FindAsync(id);

            if (NewsItem != null)
            {
                _context.Newses.Remove(NewsItem);
                await _context.SaveChangesAsync();
            }

            return Redirect(Url.GetLocalUrl(ReturnUrl));
        }
    }
}
