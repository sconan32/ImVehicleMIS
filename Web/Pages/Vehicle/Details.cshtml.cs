﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImVehicleCore.Data;

namespace Web.Pages.Vehicle
{
    public class DetailsModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;

        public DetailsModel(ImVehicleCore.Data.VehicleDbContext context)
        {
            _context = context;
        }

        public VehicleItem VehicleItem { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
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
            return Page();
        }
    }
}
