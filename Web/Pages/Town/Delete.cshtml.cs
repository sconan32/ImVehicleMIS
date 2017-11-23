using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImVehicleCore.Data;

namespace Web.Pages.Towns
{
    public class DeleteModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;

        public DeleteModel(ImVehicleCore.Data.VehicleDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TownItem TownItem { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TownItem = await _context.Towns.SingleOrDefaultAsync(m => m.Id == id);

            if (TownItem == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TownItem = await _context.Towns.FindAsync(id);

            if (TownItem != null)
            {
                _context.Towns.Remove(TownItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
