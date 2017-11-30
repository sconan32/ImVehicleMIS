using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImVehicleCore.Data;

namespace Web.Pages.Secureman
{
    public class DetailsModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;

        public DetailsModel(ImVehicleCore.Data.VehicleDbContext context)
        {
            _context = context;
        }

        public SecurityPerson SecurityPerson { get; set; }

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
