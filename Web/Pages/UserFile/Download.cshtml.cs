using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Web.Pages.UserFile
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


        public async Task<IActionResult> OnGetAsync(long id)
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
                        uf.DownloadCount += 1;
                        _context.Files.Attach(uf).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        await _context.SaveChangesAsync();
                        return File(fs, uf.ContentType, uf.FileName);

                    }
                }
            }
            catch (Exception )
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