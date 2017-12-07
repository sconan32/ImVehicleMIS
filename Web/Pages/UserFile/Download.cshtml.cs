using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Socona.ImVehicle.Core.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Web.Pages.UserFile
{
    public class DownloadModel : PageModel
    {
        private readonly VehicleDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public DownloadModel(VehicleDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }


        public IActionResult OnGet(long id)
        {
            var uf = _context.Files.FirstOrDefault(t => t.Id == id);
            try
            {
                if (uf != null)
                {
                    var svrPath = System.IO.Path.Combine(_hostingEnvironment.WebRootPath,"upload", uf.ServerPath);
                    if (System.IO.File.Exists(svrPath))
                    {
                        FileStream fs = new FileStream(svrPath, FileMode.Open, FileAccess.Read);

                        return File(fs, uf.ContentType, uf.FileName);

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return NotFound();

        }

        [BindProperty]
        public UserFileItem UserFileItem { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Files.Add(UserFileItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}