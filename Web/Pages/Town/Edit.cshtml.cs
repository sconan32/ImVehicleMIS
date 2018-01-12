using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Web.Pages.Town
{
    public class EditModel : PageModel
    {
        private readonly VehicleDbContext _context;

        public EditModel(Socona.ImVehicle.Core.Data.VehicleDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TownItem TownItem { get; set; }
        [Authorize(Roles = "Admins")]
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
        [Authorize(Roles = "Admins")]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TownItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TownItemExists(TownItem.Id))
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

        private bool TownItemExists(long id)
        {
            return _context.Towns.Any(e => e.Id == id);
        }
    }
}
