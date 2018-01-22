using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Extensions;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Core.Specifications;
using Socona.ImVehicle.Infrastructure.Specifications;

namespace Socona.ImVehicle.Web.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<VehicleUser> _signInManager;
        private readonly UserManager<VehicleUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ITownService _townService;
        private readonly IGroupService _groupService;

        public RegisterModel(
            UserManager<VehicleUser> userManager,
            SignInManager<VehicleUser> signInManager,
            ILogger<LoginModel> logger,
            IEmailSender emailSender, 
            ITownService townService,
            IGroupService groupService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _townService = townService;
            _groupService = groupService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
           

            [Required]
      
            [Display(Name = "用户名")]
            public string Name { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "邮件")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "密码")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "确认密码")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            [Display(Name = "用户角色")]
            public string RoleType { get; set; }
            [Display(Name = "街道")]
            public long? TownId { get; set; }
            [Display(Name = "安全组")]
            public long? GroupId { get; set; }

            [Display(Name = "电话")]
            public string PhoneNumber { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            var availableTowns = await Town4UserSpecification.CreateAsync(HttpContext.User, _userManager);

            var townlist = await _townService.ListAsync(availableTowns);

            ViewData["TownList"] = new SelectList(townlist, "Id", "Name");

            var availableGroups = await Group4UserSpecification.CreateAsync(HttpContext.User, _userManager);

            var groups = await _groupService.ListAsync(availableGroups);
            ViewData["GroupList"] = new SelectList(groups, "Id", "Name");
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roleList = new List<VehicleRole>();
            if (await _userManager.IsInRoleAsync(user, "GlobalVisitor") ||
               await _userManager.IsInRoleAsync(user, "Admins"))
            {
                roleList.Add(new VehicleRole() { Name = "TownManager", LocalName = "街道管理员" });
            }
            roleList.Add(new VehicleRole() { Name = "GroupManager", LocalName="安全组管理员" });
            ViewData["RoleList"] = new SelectList(roleList, "Name", "LocalName");

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new VehicleUser {
                    UserName = Input.Name,
                    Email = Input.Email,
                    Type = Input.RoleType,
                    PhoneNumber = Input.PhoneNumber,
                    Status = StatusType.OK,
                    CreationDate = DateTime.Now,
                };
                if (Input.RoleType == "TownManager")
                {
                    user.TownId = Input.TownId;
                }
                else if(Input.RoleType=="GroupManager")
                {
                    user.TownId = Input.TownId;
                    user.GroupId = Input.GroupId;
                }
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    if (Input.RoleType == "TownManager")
                    {
                       await  _userManager.AddToRoleAsync(user, "TownManager");
                    }
                    else if (Input.RoleType == "GroupManager")
                    {
                        await _userManager.AddToRoleAsync(user, "GroupManager");
                    }
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    await _emailSender.SendEmailConfirmationAsync(Input.Email, callbackUrl);

                   // await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(Url.GetLocalUrl(returnUrl));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
