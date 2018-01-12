using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Web.ViewModels;

namespace Socona.ImVehicle.Web.Pages.UserFile
{
    public class CreateModel : PageModel
    {
        private readonly VehicleDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public CreateModel(VehicleDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            
        }

        public IActionResult OnGet(long? townId,long? groupId, string returnUrl)
        {

            UserFile = new UserFileEditViewModel();
           if(townId!=null)
            {
                UserFile.TownId = townId;
                UserFile.Visibility = VisibilityType.CurrentTown;
            }
           else if(groupId!=null)
            {
                UserFile.GroupId = groupId;
                UserFile.Visibility = VisibilityType.CurrentGroup;
            }
            else
            {
                UserFile.Visibility = VisibilityType.Global;
            }
            ReturnUrl = returnUrl;

            return Page();

        }

        [BindProperty(SupportsGet =true)]
        public UserFileEditViewModel UserFile { get; set; }
        [BindProperty]
        public string ReturnUrl { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
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
                    TownId = UserFile.TownId,
                    GroupId = UserFile.GroupId,
                    Visibility = UserFile.Visibility,
                    FileName = Path.GetFileName(UserFile.UploadFile?.FileName),
                    ContentType = UserFile.UploadFile?.ContentType,
                    ServerPath = serverFileName,
                    ClientPath = UserFile.UploadFile?.FileName,
                    Size = UserFile.UploadFile?.Length ?? 0,
                    Name = UserFile.Name,
                    Type = Path.GetExtension(UserFile.UploadFile?.FileName),

                    CreationDate = DateTime.Now,
                };

                

                _context.Files.Add(ufile);
                await _context.SaveChangesAsync();

                return Redirect(Url.GetLocalUrl(ReturnUrl));
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}