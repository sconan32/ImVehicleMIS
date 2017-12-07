using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.Secureman
{
    public class EditModel : PageModel
    {
        private readonly VehicleDbContext _context;

        public EditModel(VehicleDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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
           ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id");
           ViewData["TownId"] = new SelectList(_context.Towns, "Id", "Id");
            return Page();
        }

        [Authorize(Roles = "Admins")]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SecurityPerson).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SecurityPersonExists(SecurityPerson.Id))
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

        private bool SecurityPersonExists(long id)
        {
            return _context.SecurityPersons.Any(e => e.Id == id);
        }
    }
}
