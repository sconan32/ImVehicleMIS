using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Web.Pages.UserFile
{
    public class IndexModel : PageModel
    {
        private readonly VehicleDbContext _context;

        public IndexModel(VehicleDbContext context)
        {
            _context = context;
        }

        public IList<UserFileItem> UserFile { get;set; }


        [Authorize(Roles = "Admins")]
        public async Task OnGetAsync()
        {
            UserFile = await _context.Files.ToListAsync();
        }
    }
}
