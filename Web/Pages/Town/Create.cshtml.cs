using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Web.Pages.Town
{
    public class CreateModel : PageModel
    {
        private readonly Socona.ImVehicle.Core.Data.VehicleDbContext _context;

        public CreateModel(Socona.ImVehicle.Core.Data.VehicleDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admins")]
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public TownItem TownItem { get; set; }
        [Authorize(Roles = "Admins")]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Towns.Add(TownItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}