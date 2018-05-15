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

namespace Socona.ImVehicle.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<VehicleUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<VehicleUser> _userManager;
        INewsService _newsRepisitory;
        public LoginModel(SignInManager<VehicleUser> signInManager, UserManager<VehicleUser> userManager, INewsService newsRepository, ILogger<LoginModel> logger)
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


            public string ImageBase64 { get; set; }
            public NewsListView(NewsItem news = null)
            {
                Id = news.Id;
                Name = news.Title;
                Date = news.PublishDate;
                ImageBase64 = news.Metadata;

            }
        }


        public string ReturnUrl { get; set; }


        public List<NewsListView> NewsList { get; set; } = new List<NewsListView>();
        public List<NewsListView> LawList { get; set; } = new List<NewsListView>();
        public List<NewsListView> CaseList { get; set; } = new List<NewsListView>();


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


            var news = await _newsRepisitory.LoadLoginNews();

            var ImageNews = (await _newsRepisitory.LoadLoginImages()).Select(t => new NewsListView(t)).ToList();

            ViewData["Image1"] = ImageNews[0].ImageBase64;
            ViewData["Caption1"] = ImageNews[0].Name;
            ViewData["Image2"] = ImageNews[1].ImageBase64;
            ViewData["Caption2"] = ImageNews[1].Name;
            ViewData["Image3"] = ImageNews[2].ImageBase64;
            ViewData["Caption3"] = ImageNews[2].Name;
            NewsList = news.Select(o => new NewsListView(o)).ToList();
            LawList = (await _newsRepisitory.LoadLoginLaws()).Select(t => new NewsListView(t)).ToList();
            CaseList = (await _newsRepisitory.LoadLoginCases()).Select(t => new NewsListView(t)).ToList();
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
