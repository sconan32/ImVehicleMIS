using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImVehicleCore.Data;

namespace Web.Pages.News
{
    public class EditModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;

        public EditModel(ImVehicleCore.Data.VehicleDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public NewsItem NewsItem { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NewsItem = await _context.Newses.SingleOrDefaultAsync(m => m.Id == id);

            if (NewsItem == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

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

            return RedirectToPage("./Index");
        }

        private bool NewsItemExists(long id)
        {
            return _context.Newses.Any(e => e.Id == id);
        }
    }
}
