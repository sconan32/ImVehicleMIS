using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;

namespace ImVehicleMIS.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<VehicleUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<VehicleUser> _userManager;
        public LoginModel(SignInManager<VehicleUser> signInManager,UserManager<VehicleUser> userManager, IAsyncRepository<NewsItem> newsRepository, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _newsRepisitory = newsRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class NewsListView
        {
            public long Id { get; set; }

            public string Name { get; set; }

            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            public DateTime Date { get; set; }
        }


        public string ReturnUrl { get; set; }

        IAsyncRepository<NewsItem> _newsRepisitory;
        public List<NewsListView> NewsList { get; set; } = new List<NewsListView>();

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
           
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "记住我")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;


             var news = await _newsRepisitory.ListRangeAsync(0, 10);

            NewsList = news
                .Select(o => new NewsListView()
                {
                    Id = o.Id,
                    Name = o.Title,
                    Date = o.PublishDate

                }).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                   
                    return LocalRedirect(Url.GetLocalUrl(returnUrl));
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "用户名或者密码错误，请重试.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
