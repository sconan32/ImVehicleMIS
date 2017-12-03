using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImVehicleCore.Data;
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.UserFile
{
    public class IndexModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;

        public IndexModel(ImVehicleCore.Data.VehicleDbContext context)
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
