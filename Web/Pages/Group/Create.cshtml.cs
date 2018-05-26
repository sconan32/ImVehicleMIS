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
using Socona.ImVehicle.Infrastructure.Extensions;
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



            var group = new GroupItem()
            {
                CreationDate = DateTime.Now,
                CreateBy = user.Id,
                Status = StatusType.OK,
            };

            var canEdit = _authorizationService.AuthorizeAsync(HttpContext.User, group, "CanEdit");
            if (!(await canEdit).Succeeded)
            {
                return Unauthorized();
            }


            await GroupItem.FillGroupItem(group);

            if (GroupItem.MainImage != null)
            {
                group.MainImage = GroupItem.MainImage.ToUserFile("企业图片");
                _context.Files.Add(group.MainImage);
            }
            if (GroupItem.LicenseImage != null)
            {
                group.LicenseImage = GroupItem.LicenseImage.ToUserFile("证照图片");
                _context.Files.Add(group.LicenseImage);
            }
            if (GroupItem.ExtraPhoto1 != null)
            {
                group.ExtraImage1 = GroupItem.ExtraPhoto1.ToUserFile("附加图片1");
                _context.Files.Add(group.ExtraImage1);
            }
            if (GroupItem.ExtraPhoto2 != null)
            {
                group.ExtraImage2 = GroupItem.ExtraPhoto2.ToUserFile("附加图片2");
                _context.Files.Add(group.ExtraImage2);
            }
            if (GroupItem.ExtraPhoto3 != null)
            {
                group.ExtraImage3 = GroupItem.ExtraPhoto3.ToUserFile("附加图片3");
                _context.Files.Add(group.ExtraImage3);
            }
            if (GroupItem.ApplicationFile != null)
            {
                group.ApplicationFile = GroupItem.ApplicationFile.ToUserFile("安全组审批表");
                _context.Files.Add(group.ApplicationFile);
            }
            if (GroupItem.RuleFile != null)
            {
                group.RuleFile = GroupItem.RuleFile.ToUserFile("规章制度");
                _context.Files.Add(group.RuleFile);
            }
            if (GroupItem.GroupGuranteeFile != null)
            {
                group.GroupGuranteeFile = GroupItem.GroupGuranteeFile.ToUserFile("安全组责任状");
                _context.Files.Add(group.GroupGuranteeFile);
            }
            if (GroupItem.DriverGuranteeFile != null)
            {
                group.DriverGuranteeFile = GroupItem.DriverGuranteeFile.ToUserFile("安全组责任状");
                _context.Files.Add(group.DriverGuranteeFile);
            }
            _context.Groups.Add(group);

            await _context.SaveChangesAsync();
            if (GroupItem.MainImage != null)
            {
                group.MainImage.GroupId = group.Id;
                group.MainImage.TownId = group.TownId;
                group.MainImage.Visibility = VisibilityType.CurrentGroup;
                _context.Entry(group.MainImage).State = EntityState.Modified;
            }
            if (GroupItem.LicenseImage != null)
            {
                group.LicenseImage.GroupId = group.Id;
                group.LicenseImage.TownId = group.TownId;
                group.LicenseImage.Visibility = VisibilityType.CurrentGroup;
                _context.Entry(group.LicenseImage).State = EntityState.Modified;
            }
            if (GroupItem.ExtraPhoto1 != null)
            {
                group.ExtraImage1.GroupId = group.Id;
                group.ExtraImage1.TownId = group.TownId;
                group.ExtraImage1.Visibility = VisibilityType.CurrentGroup;
                _context.Entry(group.ExtraImage1).State = EntityState.Modified;
            }
            if (GroupItem.ExtraPhoto2 != null)
            {
                group.ExtraImage2.GroupId = group.Id;
                group.ExtraImage2.TownId = group.TownId;
                group.ExtraImage2.Visibility = VisibilityType.CurrentGroup;
                _context.Entry(group.ExtraImage2).State = EntityState.Modified;
            }
            if (GroupItem.ExtraPhoto3 != null)
            {
                group.ExtraImage3.GroupId = group.Id;
                group.ExtraImage3.TownId = group.TownId;
                group.ExtraImage3.Visibility = VisibilityType.CurrentGroup;
                _context.Entry(group.ExtraImage3).State = EntityState.Modified;
            }
            if (GroupItem.ApplicationFile != null)
            {
                group.ApplicationFile.GroupId = group.Id;
                group.ApplicationFile.TownId = group.TownId;
                group.ApplicationFile.Visibility = VisibilityType.CurrentGroup;
                _context.Entry(group.ApplicationFile).State = EntityState.Modified;

            }
            if (GroupItem.RuleFile != null)
            {
                group.RuleFile.GroupId = group.Id;
                group.RuleFile.TownId = group.TownId;
                group.RuleFile.Visibility = VisibilityType.CurrentGroup;
                _context.Entry(group.RuleFile).State = EntityState.Modified;
            }
            if (GroupItem.GroupGuranteeFile != null)
            {
                group.GroupGuranteeFile.GroupId = group.Id;
                group.GroupGuranteeFile.TownId = group.TownId;
                group.GroupGuranteeFile.Visibility = VisibilityType.CurrentGroup;
                _context.Entry(group.GroupGuranteeFile).State = EntityState.Modified;
            }
            if (GroupItem.DriverGuranteeFile != null)
            {
                group.DriverGuranteeFile.GroupId = group.Id;
                group.DriverGuranteeFile.TownId = group.TownId;
                group.DriverGuranteeFile.Visibility = VisibilityType.CurrentGroup;
                _context.Entry(group.DriverGuranteeFile).State = EntityState.Modified;
            }
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