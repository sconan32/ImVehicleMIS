using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Core.Specifications;
using Socona.ImVehicle.Web.ViewModels;

namespace Socona.ImVehicle.Web.Pages.Town
{
    public class IndexModel : PageModel
    {
        private readonly ITownRepository _townRepository;

        private readonly IGroupRepository _groupService;

        private readonly UserManager<VehicleUser> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public IndexModel(ITownRepository townRepository, IGroupRepository groupService, UserManager<VehicleUser> userManager,
            IAuthorizationService authorizationService)
        {
            _townRepository = townRepository;
            _groupService = groupService;
            _userManager = userManager;
            _authorizationService=authorizationService;
        }

        public List<TownListViewModel> TownList { get; set; }

        public async Task<bool> CanEdit()
        {
            var gv = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireGlobalVisitorRole");
            var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await gv).Succeeded || (await admin).Succeeded;
        }

        public async Task OnGetAsync()
        {

            var specification = await Town4UserSpecification.CreateAsync(HttpContext.User, _userManager);
            specification.Includes.Add(t => t.Drivers);
            specification.Includes.Add(t => t.Vehicles);
            specification.Includes.Add(t => t.Groups);

            var towns = await _townRepository.ListAsync(specification);
            TownList = towns.OrderBy(t => t.Code).Select(t =>
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
        }
    }
}
