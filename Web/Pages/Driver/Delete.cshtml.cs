﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.Driver
{
    public class DeleteModel : PageModel
    {
        private readonly Socona.ImVehicle.Core.Data.VehicleDbContext _context;

        public DeleteModel(Socona.ImVehicle.Core.Data.VehicleDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string ReturnUrl { get; set; }

        [BindProperty]
        public DriverItem DriverItem { get; set; }
        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> OnGetAsync(long? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }
            ReturnUrl = returnUrl;
            DriverItem = await _context.Drivers.SingleOrDefaultAsync(m => m.Id == id);

            if (DriverItem == null)
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

            DriverItem = await _context.Drivers.FindAsync(id);

            if (DriverItem != null)
            {
                _context.Drivers.Remove(DriverItem);
                await _context.SaveChangesAsync();
            }

            return Redirect(Url.GetLocalUrl(ReturnUrl));
        }
    }
}
