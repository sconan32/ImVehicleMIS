﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Web.Pages.Town
{
    public class DeleteModel : PageModel
    {
        private readonly Socona.ImVehicle.Core.Data.VehicleDbContext _context;

        public DeleteModel(Socona.ImVehicle.Core.Data.VehicleDbContext context)
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
