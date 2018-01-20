using System;
using System.Collections.Generic;
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
    public class EditModel : PageModel
    {
        private readonly VehicleDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<VehicleUser> _userManager;
        private readonly ITownService _townService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public EditModel(VehicleDbContext context, IAuthorizationService authorizationService,
            UserManager<VehicleUser> userManager, ITownService townService,
            IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _authorizationService = authorizationService;
            _userManager = userManager;
            _townService = townService;
            _hostingEnvironment = hostingEnvironment;
        }
        [BindProperty]
        public string ReturnUrl { get; set; }
        [BindProperty]
        public GroupViewModel GroupItem { get; set; }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> OnGetAsync(long? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            ReturnUrl = returnUrl;
            var group = await _context.Groups.SingleOrDefaultAsync(m => m.Id == id);
            if (group == null)
            {
                return NotFound();
            }
            var canEdit = _authorizationService.AuthorizeAsync(HttpContext.User, group, "CanEdit");
            if (!(await canEdit).Succeeded)
            {
                return Unauthorized();
            }


            TownList = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User))
           .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name, })
           .ToList();

            GroupItem = new GroupViewModel(group);

            return Page();
        }

        public List<SelectListItem> TownList { get; set; }

        public async Task<bool> IsAdmin()
        {
            var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await admin).Succeeded;
        }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }


            var group = _context.Groups.FirstOrDefault(t => t.Id == GroupItem.Id);
            if (group == null)
            {
                return NotFound();
            }

            await GroupItem.FillGroupItem(group);

            var canEdit = _authorizationService.AuthorizeAsync(HttpContext.User, group, "CanEdit");
            if (!(await canEdit).Succeeded)
            {
                return Unauthorized();
            }
            if(GroupItem.ApplicationFile!=null)
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


            group.ModificationDate = DateTime.Now;
            group.ModifyBy = (await _userManager.GetUserAsync(HttpContext.User)).Id;
            group.Status = StatusType.OK;
            group.VersionNumber += 1;

            _context.Attach(group).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupItemExists(GroupItem.Id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Redirect(Url.GetLocalUrl(ReturnUrl));
        }

        private bool GroupItemExists(long id)
        {
            return _context.Groups.Any(e => e.Id == id);
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
