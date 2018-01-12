using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Web.Pages.Secureman
{
    public class DetailsModel : PageModel
    {
        private readonly VehicleDbContext _context;

        public DetailsModel(VehicleDbContext context)
        {
            _context = context;
        }


        public SecurityPerson SecurityPerson { get; set; }

        [Authorize(Roles = "Admins")]
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SecurityPerson = await _context.SecurityPersons
                .Include(s => s.Group)
                .Include(s => s.Town).SingleOrDefaultAsync(m => m.Id == id);

            if (SecurityPerson == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
