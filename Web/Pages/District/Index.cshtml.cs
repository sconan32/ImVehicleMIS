using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Web.Pages.District
{
    public class IndexModel : PageModel
    {
        private readonly Socona.ImVehicle.Core.Data.VehicleDbContext _context;

        public IndexModel(Socona.ImVehicle.Core.Data.VehicleDbContext context)
        {
            _context = context;
        }

        public IList<DistrictItem> DistrictItem { get;set; }
        [Authorize(Roles = "Admins")]
        public async Task OnGetAsync()
        {
            DistrictItem = await _context.Districts.ToListAsync();
        }
    }
}
