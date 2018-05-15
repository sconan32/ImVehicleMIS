using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Web.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<VehicleUser> _userManager;
        private readonly SignInManager<VehicleUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;
        private readonly VehicleDbContext _dbContext;

        public ChangePasswordModel(
            UserManager<VehicleUser> userManager,
            SignInManager<VehicleUser> signInManager, VehicleDbContext dbContext,
        ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {

            public string Id { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "当前密码")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "新密码")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "重复新密码")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        //public async Task<IActionResult> OnGetAsync()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        //    }

        //    var hasPassword = await _userManager.HasPasswordAsync(user);
        //    if (!hasPassword)
        //    {
        //        return RedirectToPage("./SetPassword");
        //    }
        //    ViewData["Id"] = user.Id;
        //    return Page();
        //}

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }

                var hasPassword = await _userManager.HasPasswordAsync(user);
                if (!hasPassword)
                {
                    return RedirectToPage("./SetPassword");
                }
                ViewData["Id"] = user.Id;
                return Page();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }

                UserStore<VehicleUser> store = new UserStore<VehicleUser>(_dbContext);
                VehicleUser cUser = await store.FindByIdAsync(id);

                if (cUser != null)
                {
                    if (cUser.TownId == user.TownId && User.IsInRole("TownManager")
                        || User.IsInRole("Admins") || User.IsInRole("GlobalVisitor"))
                    {
                        ViewData["Id"] = id;
                        return Page();
                    }
                }
            }
            return NotFound();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (user.Id == Input.Id)
            {


                var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation("User changed their password successfully.");
                StatusMessage = "密码已经成功修改";
            }
            else
            {
                UserStore<VehicleUser> store = new UserStore<VehicleUser>(_dbContext);
                VehicleUser cUser = await store.FindByIdAsync(Input.Id);

                if (cUser != null)
                {

                    if (cUser.TownId == user.TownId && User.IsInRole("TownManager")
                        || User.IsInRole("Admins") || User.IsInRole("GlobalVisitor"))
                    {

                        String newPassword = Input.NewPassword;
                        String hashedNewPassword = _userManager.PasswordHasher.HashPassword(cUser, newPassword);

                        await store.SetPasswordHashAsync(cUser, hashedNewPassword);
                        await store.UpdateAsync(cUser);
                        StatusMessage = "密码已经成功修改";
                    }

                }
            }
            return RedirectToPage();
        }
    }
}
