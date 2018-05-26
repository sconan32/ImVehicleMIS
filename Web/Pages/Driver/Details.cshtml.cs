using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Web.ViewModels;
using System.Collections.Generic;

namespace Socona.ImVehicle.Web.Pages.Driver
{
    public class DetailsModel : PageModel
    {
        private readonly Socona.ImVehicle.Core.Data.VehicleDbContext _context;
        private readonly ITownRepository _townRepository;
        private readonly IAuthorizationService _authorizationService;

        IGroupRepository _groupService;
        public DetailsModel(Socona.ImVehicle.Core.Data.VehicleDbContext context, ITownRepository townRepository, IGroupRepository groupService, IAuthorizationService authorizationService)
        {
            _townRepository = townRepository;
            _groupService = groupService;
            _authorizationService = authorizationService;
            _context = context;
        }
        public async Task<bool> CanEdit()
        {
            var tm = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireTownManagerRole");
            var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await tm).Succeeded || (await admin).Succeeded;
        }

        public DriverViewModel DriverItem { get; set; }
        public List<VehicleListViewModel> Vehicles { get; set; }
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .Include(t => t.Vehicles).ThenInclude(v => v.Town)
                .Include(t => t.Vehicles).ThenInclude(v => v.Group)
                .Include(t => t.Town)
                .Include(t => t.Group)
                .Include(t=>t.AvatarImage)
                .Include(t=>t.LicenseImage)
                .Include(t=>t.ExtraImage1)
                .Include(t => t.ExtraImage2)
                .Include(t => t.ExtraImage3)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (driver == null)
            {
                return NotFound();
            }
            DriverItem = new DriverViewModel(driver);
            Vehicles = driver.Vehicles.Select(t => new VehicleListViewModel(t)).ToList();
            return Page();
        }
    }
}
