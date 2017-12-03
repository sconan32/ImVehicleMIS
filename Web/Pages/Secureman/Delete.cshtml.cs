using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImVehicleCore.Data;
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.Secureman
{
    public class DeleteModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;

        public DeleteModel(ImVehicleCore.Data.VehicleDbContext context)
        {
            _context = context;
        }

        [BindProperty]

        public string ReturnUrl { get; set; }
        [BindProperty]
        public SecurityPerson SecurityPerson { get; set; }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> OnGetAsync(long? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            SecurityPerson = await _context.SecurityPersons
                .Include(s => s.Group)
                .Include(s => s.Town).SingleOrDefaultAsync(m => m.Id == id);
            ReturnUrl = returnUrl;
            if (SecurityPerson == null)
            {
                return NotFound();
            }
            return Page();
        }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SecurityPerson = await _context.SecurityPersons.FindAsync(id);

            if (SecurityPerson != null)
            {
                _context.SecurityPersons.Remove(SecurityPerson);
                await _context.SaveChangesAsync();
            }

            return Redirect(Url.GetLocalUrl(ReturnUrl));
        }
    }
}
