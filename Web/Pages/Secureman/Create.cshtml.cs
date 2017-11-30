using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImVehicleCore.Data;

namespace Web.Pages.Secureman
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
        ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id");
        ViewData["TownId"] = new SelectList(_context.Towns, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public SecurityPerson SecurityPerson { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.SecurityPersons.Add(SecurityPerson);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}