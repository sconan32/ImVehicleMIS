using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImVehicleCore.Data;

namespace Web.Pages.News
{
    public class CreateModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;

        public CreateModel(ImVehicleCore.Data.VehicleDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string ReturnUrl { get; set; }
        public IActionResult OnGet(string returnUrl)
        {

            ReturnUrl = returnUrl;
            return Page();
        }

        [BindProperty]
        public NewsItem NewsItem { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Newses.Add(NewsItem);
            await _context.SaveChangesAsync();

            return Redirect(Url.GetLocalUrl(ReturnUrl));
        }
    }
}