using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Socona.ImVehicle.Core.Data;
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.Towns
{
    public class CreateModel : PageModel
    {
        private readonly Socona.ImVehicle.Core.Data.VehicleDbContext _context;

        public CreateModel(Socona.ImVehicle.Core.Data.VehicleDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admins")]
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public TownItem TownItem { get; set; }
        [Authorize(Roles = "Admins")]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Towns.Add(TownItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}