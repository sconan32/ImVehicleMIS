using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.Vehicle
{
    public class DeleteModel : PageModel
    {
        private readonly VehicleDbContext _context;

        public DeleteModel(VehicleDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public VehicleItem VehicleItem { get; set; }
        [BindProperty]
        public string ReturnUrl { get; set; }


        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> OnGetAsync(long? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            VehicleItem = await _context.Vehicles.SingleOrDefaultAsync(m => m.Id == id);

            if (VehicleItem == null)
            {
                return NotFound();
            }
            ReturnUrl = returnUrl;
            return Page();
        }
        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VehicleItem = await _context.Vehicles.FindAsync(id);

            if (VehicleItem != null)
            {
                _context.Vehicles.Remove(VehicleItem);
                await _context.SaveChangesAsync();
            }

            return Redirect(Url.GetLocalUrl(ReturnUrl));
        }
    }
}
