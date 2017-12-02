using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImVehicleCore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Web.Pages.News
{
    public class CreateModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;
        private readonly UserManager<VehicleUser> _userManager;
        public CreateModel(ImVehicleCore.Data.VehicleDbContext context,UserManager<VehicleUser> userManager)
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
        public NewsItem NewsItem { get; set; }
        [Authorize(Roles = "GlobalVisitor,Admins")]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

           
            var user = await _userManager.GetUserAsync(HttpContext.User);
            NewsItem.CreationDate = DateTime.Now;
            NewsItem.CreateBy = user.Id;

            _context.Newses.Add(NewsItem);
            await _context.SaveChangesAsync();

            return Redirect(Url.GetLocalUrl(ReturnUrl));
        }
    }
}