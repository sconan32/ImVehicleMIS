using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Web.Pages.District
{
    public class EditModel : PageModel
    {
        private readonly Socona.ImVehicle.Core.Data.VehicleDbContext _context;

        public EditModel(Socona.ImVehicle.Core.Data.VehicleDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DistrictItem DistrictItem { get; set; }
        [Authorize(Roles = "Admins")]
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DistrictItem = await _context.Districts.SingleOrDefaultAsync(m => m.Id == id);

            if (DistrictItem == null)
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

            _context.Attach(DistrictItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistrictItemExists(DistrictItem.Id))
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

        private bool DistrictItemExists(long id)
        {
            return _context.Districts.Any(e => e.Id == id);
        }
    }
}
