using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Infrastructure.Interfaces;
using Socona.ImVehicle.Web.ViewModels;

namespace Socona.ImVehicle.Web.Pages.Town
{
    public class DetailsModel : PageModel
    {
        private readonly IUserFileService _userFileService;
        private readonly ITownRepository _townRepository;
        private readonly IAuthorizationService _authorizationService;

        IGroupRepository _groupService;
        public DetailsModel(ITownRepository townRepository, IGroupRepository groupService,
            IAuthorizationService authorizationService, IUserFileService userFileService)
        {
            _townRepository = townRepository;
            _groupService = groupService;
            _authorizationService = authorizationService;
            _userFileService = userFileService;
        }

        public TownDetailViewModel TownItem { get; set; }

        public List<UserFileListViewModel> GlobalFiles { get; set; }

        public List<UserFileListViewModel> UserFiles { get; set; }

        public async Task<bool> CanEdit()
        {
            var tm = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireTownManagerRole");
            var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await tm).Succeeded || (await admin).Succeeded;
        }


        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var town = await _townRepository.GetByIdEagerAsync(id.Value);
            if (town == null)
            {
                return NotFound();
            }
            TownItem = new TownDetailViewModel(town);
            GlobalFiles = (await _userFileService.GetGlobalUserFilesAsync()).Select(t => new UserFileListViewModel(t)).ToList();
            UserFiles=(await _userFileService.GetUserFilesForTownAsync(id.Value)).Select(t => new UserFileListViewModel(t)).ToList();

            return Page();
        }
    }
}
