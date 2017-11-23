using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImVehicleCore.Data;

namespace Web.Pages.Driver
{
    public class CreateModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;

        public CreateModel(ImVehicleCore.Data.VehicleDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public DriverItem DriverItem { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Drivers.Add(DriverItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}