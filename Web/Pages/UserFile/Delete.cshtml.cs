﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Web.Pages.UserFile
{
    public class DeleteModel : PageModel
    {
        private readonly VehicleDbContext _context;

        public DeleteModel(VehicleDbContext context)
        {
            _context = context;
        }

        [BindProperty]

        public string ReturnUrl { get; set; }


        [BindProperty]
        public UserFileItem UserFile { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id ,string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }
            ReturnUrl = returnUrl;
            UserFile = await _context.Files.SingleOrDefaultAsync(m => m.Id == id);

            if (UserFile == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserFile = await _context.Files.FindAsync(id);

            if (UserFile != null)
            {
                _context.Files.Remove(UserFile);
                try
                {
                    System.IO.File.Delete(UserFile.ServerPath);
                }
                catch(Exception ex)
                {

                }
                await _context.SaveChangesAsync();
            }

            return Redirect(Url.GetLocalUrl(ReturnUrl));
        }
    }
}
