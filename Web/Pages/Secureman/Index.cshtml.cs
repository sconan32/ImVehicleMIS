using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Web.Pages.Secureman
{
    public class IndexModel : PageModel
    {
        private readonly VehicleDbContext _context;

        public IndexModel(VehicleDbContext context)
        {
            _context = context;
        }

        public IList<SecurityPerson> SecurityPerson { get;set; }
        [Authorize(Roles = "Admins")]
        public async Task OnGetAsync()
        {
            SecurityPerson = await _context.SecurityPersons
                .Include(s => s.Group)
                .Include(s => s.Town).ToListAsync();
        }
    }
}
