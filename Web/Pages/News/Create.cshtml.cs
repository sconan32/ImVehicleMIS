using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Web.ViewModels;

namespace Socona.ImVehicle.Web.Pages.News
{
    public class CreateModel : PageModel
    {
        private readonly VehicleDbContext _context;
        private readonly UserManager<VehicleUser> _userManager;
        public CreateModel(Socona.ImVehicle.Core.Data.VehicleDbContext context,UserManager<VehicleUser> userManager)
        {
            _context = context;
            _userManager = userManager;
           
        }

        [BindProperty]
        public string ReturnUrl { get; set; }


        [Authorize(Roles = "GlobalVisitor,Admins")]
        public IActionResult OnGet(string returnUrl)
        {

            ReturnUrl = returnUrl;
           
            return Page();
        }

        [BindProperty]
        public NewsEditViewModel NewsItem { get; set; }
        [Authorize(Roles = "GlobalVisitor,Admins")]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

           
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var news = new NewsItem();
            await NewsItem.FillNewsItem(news);
            news.CreationDate = DateTime.Now;
            news.CreateBy = user.Id;
            news.VersionNumber = 1;

            _context.Newses.Add(news);
            await _context.SaveChangesAsync();

            return Redirect(Url.GetLocalUrl(ReturnUrl));
        }
       
    }
}