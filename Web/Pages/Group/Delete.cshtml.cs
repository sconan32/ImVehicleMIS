using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImVehicleCore.Data;

namespace Web.Pages.Group
{
    public class DeleteModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;

        public DeleteModel(ImVehicleCore.Data.VehicleDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public GroupItem GroupItem { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GroupItem = await _context.Groups.SingleOrDefaultAsync(m => m.Id == id);

            if (GroupItem == null)
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

            GroupItem = await _context.Groups.FindAsync(id);

            if (GroupItem != null)
            {
                _context.Groups.Remove(GroupItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
