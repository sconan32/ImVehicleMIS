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
using Socona.ImVehicle.Infrastructure.Extensions;
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

            var group = _context.Groups.Include(t=>t.MainImage).FirstOrDefault(t => t.Id == GroupItem.Id);
            if (group == null)
            {
                return NotFound();
            }      

            var canEdit = _authorizationService.AuthorizeAsync(HttpContext.User, group, "CanEdit");
            if (!(await canEdit).Succeeded)
            {
                return Unauthorized();
            }

            await GroupItem.FillGroupItem(group);

            if (GroupItem.MainImage != null)
            {
                if (group.MainImage?.Status == StatusType.OK)
                {                
                    group.MainImage.DeleteFromServer();
                    group.MainImage.Status = StatusType.Deleted;
                    _context.Entry(group.MainImage).State = EntityState.Modified;
                }
                group.MainImage = GroupItem.MainImage.ToUserFile("企业图片");
                group.MainImage.GroupId = group.Id;
                group.MainImage.TownId = group.TownId;
                group.MainImage.Visibility = VisibilityType.CurrentGroup;
                _context.Files.Add(group.MainImage);
            }
            if (GroupItem.LicenseImage != null)
            {
                if (group.LicenseImage?.Status == StatusType.OK)
                {
                    group.LicenseImage.DeleteFromServer();
                    group.LicenseImage.Status = StatusType.Deleted;
                    _context.Entry(group.LicenseImage).State = EntityState.Modified;
                }
                group.LicenseImage = GroupItem.LicenseImage.ToUserFile("证照图片");
                group.LicenseImage.GroupId = group.Id;
                group.LicenseImage.TownId = group.TownId;
                group.LicenseImage.Visibility = VisibilityType.CurrentGroup;
                _context.Files.Add(group.LicenseImage);
            }
            if (GroupItem.ExtraPhoto1 != null)
            {
                if (group.ExtraImage1?.Status == StatusType.OK)
                {
                    group.ExtraImage1.DeleteFromServer();
                    group.ExtraImage1.Status = StatusType.Deleted;
                    _context.Entry(group.ExtraImage1).State = EntityState.Modified;
                }
                group.ExtraImage1 = GroupItem.ExtraPhoto1.ToUserFile("附加图片1");
                group.ExtraImage1.GroupId = group.Id;
                group.ExtraImage1.TownId = group.TownId;
                group.ExtraImage1.Visibility = VisibilityType.CurrentGroup;
                _context.Files.Add(group.ExtraImage1);
            }
            if (GroupItem.ExtraPhoto2 != null)
            {
                if (group.ExtraImage2?.Status == StatusType.OK)
                {
                    group.ExtraImage2.DeleteFromServer();
                    group.ExtraImage2.Status = StatusType.Deleted;
                    _context.Entry(group.ExtraImage2).State = EntityState.Modified;
                }
                group.ExtraImage2 = GroupItem.ExtraPhoto2.ToUserFile("附加图片2");
                group.ExtraImage2.GroupId = group.Id;
                group.ExtraImage2.TownId = group.TownId;
                group.ExtraImage2.Visibility = VisibilityType.CurrentGroup;
                _context.Files.Add(group.ExtraImage2);
            }
            if (GroupItem.ExtraPhoto3 != null)
            {
                if (group.ExtraImage3?.Status == StatusType.OK)
                {
                    group.ExtraImage3.DeleteFromServer();
                    group.ExtraImage3.Status = StatusType.Deleted;
                    _context.Entry(group.ExtraImage3).State = EntityState.Modified;
                }
                group.ExtraImage3 = GroupItem.ExtraPhoto3.ToUserFile("附加图片3");
                group.ExtraImage3.GroupId = group.Id;
                group.ExtraImage3.TownId = group.TownId;
                group.ExtraImage3.Visibility = VisibilityType.CurrentGroup;
                _context.Files.Add(group.ExtraImage3);
            }
            if (GroupItem.ApplicationFile != null)
            {
                if (group.ApplicationFile?.Status == StatusType.OK)
                {
                    group.ApplicationFile.DeleteFromServer();
                    group.ApplicationFile.Status = StatusType.Deleted;
                    _context.Entry(group.ApplicationFile).State = EntityState.Modified;
                }
                group.ApplicationFile = GroupItem.ApplicationFile.ToUserFile("安全组审批表");
                group.ApplicationFile.GroupId = group.Id;
                group.ApplicationFile.TownId = group.TownId;
                group.ApplicationFile.Visibility = VisibilityType.CurrentGroup;
                _context.Files.Add(group.ApplicationFile);
            }
            if (GroupItem.RuleFile != null)
            {
                if (group.RuleFile?.Status == StatusType.OK)
                {
                    group.RuleFile.DeleteFromServer();
                    group.RuleFile.Status = StatusType.Deleted;
                    _context.Entry(group.RuleFile).State = EntityState.Modified;
                }
                group.RuleFile = GroupItem.RuleFile.ToUserFile("规章制度");
                group.RuleFile.GroupId = group.Id;
                group.RuleFile.TownId = group.TownId;
                group.RuleFile.Visibility = VisibilityType.CurrentGroup;
                _context.Files.Add(group.RuleFile);
            }
            if (GroupItem.GroupGuranteeFile != null)
            {
                if (group.GroupGuranteeFile?.Status == StatusType.OK)
                {
                    group.GroupGuranteeFile.DeleteFromServer();
                    group.GroupGuranteeFile.Status = StatusType.Deleted;
                    _context.Entry(group.GroupGuranteeFile).State = EntityState.Modified;
                }
                group.GroupGuranteeFile = GroupItem.GroupGuranteeFile.ToUserFile("安全组责任状");
                group.GroupGuranteeFile.GroupId = group.Id;
                group.GroupGuranteeFile.TownId = group.TownId;
                group.GroupGuranteeFile.Visibility = VisibilityType.CurrentGroup;
                _context.Files.Add(group.GroupGuranteeFile);
            }
            if (GroupItem.DriverGuranteeFile != null)
            {
                if (group.DriverGuranteeFile?.Status == StatusType.OK)
                {
                    group.DriverGuranteeFile.DeleteFromServer();
                    group.DriverGuranteeFile.Status = StatusType.Deleted;
                    _context.Entry(group.DriverGuranteeFile).State = EntityState.Modified;
                }
                group.DriverGuranteeFile = GroupItem.DriverGuranteeFile.ToUserFile("驾驶员责任状");
                group.DriverGuranteeFile.GroupId = group.Id;
                group.DriverGuranteeFile.TownId = group.TownId;
                group.DriverGuranteeFile.Visibility = VisibilityType.CurrentGroup;
                _context.Files.Add(group.DriverGuranteeFile);
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
