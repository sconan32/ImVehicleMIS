using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImVehicleCore.Data;
using Web.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Web.Pages.UserFile
{
    public class CreateModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public CreateModel(ImVehicleCore.Data.VehicleDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            UserFile = new UserFileEditViewModel();
        }

        public IActionResult OnGet(long groupId)
        {

            UserFile.GroupId = groupId;
            ReturnUrl = "/Group/Details?Id=" + groupId;

            return Page();

        }

        [BindProperty]
        public UserFileEditViewModel UserFile { get; set; }
        [BindProperty]
        public string ReturnUrl { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (UserFile.UploadFile == null)
            {
                return Page();
            }

            try
            {
                string serverFileName = Guid.NewGuid().ToString() + ".ufile";
                string serverPath = Path.Combine(_hostingEnvironment.WebRootPath, "upload", serverFileName);

                FileStream fileToWrite = new FileStream(serverPath, FileMode.Create, FileAccess.Write);
                await UserFile.UploadFile.CopyToAsync(fileToWrite);
                fileToWrite.Close();
                var ufile = new UserFileItem()
                {
                    GroupId = UserFile.GroupId,
                    FileName = Path.GetFileName(UserFile.UploadFile?.FileName),
                    ContentType = UserFile.UploadFile?.ContentType,
                    ServerPath = serverFileName,
                    ClientPath = UserFile.UploadFile?.FileName,
                    Size = UserFile.UploadFile?.Length ?? 0,
                    Name = UserFile.Name,
                    Type = Path.GetExtension(UserFile.UploadFile?.FileName),


                };



                _context.Files.Add(ufile);
                await _context.SaveChangesAsync();

                return RedirectToPage(Url.GetLocalUrl(ReturnUrl));
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}