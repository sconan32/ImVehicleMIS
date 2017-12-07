﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.News
{
    public class DetailsModel : PageModel
    {
        private readonly VehicleDbContext _context;

        public DetailsModel(VehicleDbContext context)
        {
            _context = context;
        }

        public NewsItem NewsItem { get; set; }

        [AllowAnonymous]
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NewsItem = await _context.Newses.SingleOrDefaultAsync(m => m.Id == id);

            if (NewsItem == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
