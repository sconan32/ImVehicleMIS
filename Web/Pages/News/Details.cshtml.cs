using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Web.ViewModels;

namespace Socona.ImVehicle.Web.Pages.News
{
    public class DetailsModel : PageModel
    {
        private readonly VehicleDbContext _context;

        public DetailsModel(VehicleDbContext context)
        {
            _context = context;
        }
  
        public NewsEditViewModel NewsItem { get; set; }

        [AllowAnonymous]
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.Newses.Include(t=>t.ImageFile).SingleOrDefaultAsync(m => m.Id == id);


            if (news == null)
            {
                return NotFound();
            }

            NewsItem = new NewsEditViewModel(news);

            return Page();
        }
    }
}
