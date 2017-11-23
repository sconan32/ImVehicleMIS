using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImVehicleCore.Data;

namespace Web.Pages.Driver
{
    public class DetailsModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;

        public DetailsModel(ImVehicleCore.Data.VehicleDbContext context)
        {
            _context = context;
        }

        public DriverItem DriverItem { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DriverItem = await _context.Drivers.SingleOrDefaultAsync(m => m.Id == id);

            if (DriverItem == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
