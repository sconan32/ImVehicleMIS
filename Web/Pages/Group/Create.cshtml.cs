using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Web.ViewModels;

namespace Socona.ImVehicle.Web.Pages.Group
{
    public class CreateModel : PageModel
    {
        private readonly VehicleDbContext _context;
        private readonly UserManager<VehicleUser> _userManager;
        private readonly ITownService _townService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public CreateModel(VehicleDbContext context, UserManager<VehicleUser> signInManager, IAuthorizationService authorizationService,
            ITownService townService, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = signInManager;
            _authorizationService = authorizationService;
            _townService = townService;
            GroupItem = new GroupViewModel(new Core.Data.GroupItem());
            _hostingEnvironment = hostingEnvironment;
        }
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> OnGetAsync(long? groupId, string returnUrl)
        {


            ReturnUrl = returnUrl;

            ViewData["TownList"] = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User))
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name, })
                .ToList();

            return Page();

        }


        [BindProperty(SupportsGet = true)]
        public GroupViewModel GroupItem { get; set; }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["TownList"] = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User))
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name, })
                .ToList();
                return Page();
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var canEdit = _authorizationService.AuthorizeAsync(HttpContext.User, "CanEdit");
            if (!(await canEdit).Succeeded)
            {
                return Unauthorized();
            }

            var group = new GroupItem()
            {
                CreationDate = DateTime.Now,
                CreateBy = user.Id,
                Status = StatusType.OK,
            };
            await GroupItem.FillGroupItem(group);


            if (GroupItem.ApplicationFile != null)
            {
                group.ApplicationFileId = await SaveUserFile(GroupItem.ApplicationFileId, GroupItem.ApplicationFile, nameof(GroupItem.ApplicationFile));
            }
            if (GroupItem.GroupGuranteeFile != null)
            {
                group.GroupGuranteeFileId = await SaveUserFile(GroupItem.GroupGuranteeFileId, GroupItem.GroupGuranteeFile, nameof(GroupItem.GroupGuranteeFile));
            }
            if (GroupItem.DriverGuranteeFile != null)
            {
                group.DriverGuranteeFileId = await SaveUserFile(GroupItem.DriverGuranteeFileId, GroupItem.DriverGuranteeFile, nameof(GroupItem.DriverGuranteeFile));
            }
            if (GroupItem.RuleFile != null)
            {
                group.RuleFileId = await SaveUserFile(GroupItem.RuleFileId, GroupItem.RuleFile, nameof(GroupItem.RuleFile));
            }

            _context.Groups.Add(group);

            await _context.SaveChangesAsync();

            return Redirect(Url.GetLocalUrl(ReturnUrl));
        }
        public async Task<bool> IsAdmin()
        {
            var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await admin).Succeeded;
        }

        private async Task<long> SaveUserFile(long? fileId, IFormFile formFile, string name)
        {

            UserFileItem userFile = null;
            if (fileId != null)
            {
                userFile = _context.Files.FirstOrDefault(f => f.Id == fileId);
                string spath = Path.Combine(_hostingEnvironment.WebRootPath, "upload", userFile.ServerPath);
                System.IO.File.Move(spath, spath + ".deleted");
            }
            if (userFile == null)
            {
                userFile = new UserFileItem();
            }
            string serverFileName = Guid.NewGuid().ToString() + ".ufile";
            string serverPath = Path.Combine(_hostingEnvironment.WebRootPath, "upload", serverFileName);

            FileStream fileToWrite = new FileStream(serverPath, FileMode.Create, FileAccess.Write);
            await formFile.CopyToAsync(fileToWrite);
            fileToWrite.Close();

            userFile.TownId = GroupItem.TownId;
            userFile.GroupId = GroupItem.Id;

            userFile.Visibility = VisibilityType.CurrentGroup;
            userFile.FileName = Path.GetFileName(formFile?.FileName);
            userFile.ContentType = formFile?.ContentType;
            userFile.ServerPath = serverFileName;
            userFile.ClientPath = formFile?.FileName;
            userFile.Size = formFile?.Length ?? 0;
            userFile.Name = name;
            userFile.Type = Path.GetExtension(formFile?.FileName);


            if (fileId != null)
            {
                _context.Attach(userFile).State = EntityState.Modified;

            }
            else
            {
                _context.Add(userFile);
            }
            await _context.SaveChangesAsync();
            return userFile.Id;
        }

    }
}