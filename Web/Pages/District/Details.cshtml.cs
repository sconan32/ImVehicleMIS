using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Core.Specifications;
using Socona.ImVehicle.Web.ViewModels;

namespace Socona.ImVehicle.Web.Pages.District
{
    public class DetailsModel : PageModel
    {
        private readonly VehicleDbContext _context;
        private readonly ITownRepository _townRepository;
        private readonly UserManager<VehicleUser> _userManager;
        public DetailsModel(VehicleDbContext context, ITownRepository townRepository, UserManager<VehicleUser> userManager)
        {
            _context = context;
            _townRepository = townRepository;
            _userManager = userManager;
        }

        public DistrictItem DistrictItem { get; set; }
        public List<TownListViewModel> Towns { get; set; }

        public List<UserFileListViewModel> UserFiles { get; set; }

        [Authorize(Roles = "Admins,GlobalVisitor")]
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DistrictItem = await _context.Districts.SingleOrDefaultAsync(m => m.Id == id);
            if (DistrictItem == null)
            {
                return NotFound();
            }
            var specification = await Town4UserSpecification.CreateAsync(HttpContext.User, _userManager);
            specification.Includes.Add(t => t.Drivers);
            specification.Includes.Add(t => t.Vehicles);
            specification.Includes.Add(t => t.Groups);

            var towns = await _townRepository.ListAsync(specification);
            Towns = towns.OrderBy(t => t.Code).Select(t =>
          new TownListViewModel()
          {
              Id = t.Id,
              Code = t.Code,
              Name = t.Name,
              GroupCount = t.Groups?.Count ?? 0,
              DriverCount = t.Drivers?.Count ?? 0,
              VehicleCount = t.Vehicles?.Count ?? 0,
              IsValid = t.IsValid(),
              StatusText = t.IsValid() ? "正常" : "预警",
          }).ToList();

            UserFiles = _context.Files.Where(t => t.Visibility == VisibilityType.Global).Select(t => new UserFileListViewModel(t)).ToList();
         
            return Page();
        }
    }
}
