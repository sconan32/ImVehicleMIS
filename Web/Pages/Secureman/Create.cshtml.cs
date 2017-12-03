using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImVehicleCore.Data;
using Microsoft.AspNetCore.Authorization;
using ImVehicleCore.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Web.Pages.Secureman
{
    public class CreateModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly ITownService _townService;
        private readonly UserManager<VehicleUser> _userManager;
        private readonly IGroupService _groupService;
        public CreateModel(ImVehicleCore.Data.VehicleDbContext context, IAuthorizationService authorizationService, ITownService townService, UserManager<VehicleUser> userManager,
            IGroupService groupService)
        {
            _context = context;
            _authorizationService = authorizationService;
            _townService = townService;
            _userManager = userManager;
            _groupService = groupService;
            SecurityPerson = new SecurityPerson();
        }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task< IActionResult> OnGetAsync(int? groupId, string returnUrl)
        {
            ReturnUrl = returnUrl;

            var townlist = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User));

            ViewData["TownList"] = new SelectList(townlist, "Id", "Name");
            if (townlist.Any())
            {
                var groups = (await _groupService.ListGroupsForTownEagerAsync(HttpContext.User, townlist.First().Id));
                ViewData["GroupList"] = new SelectList(groups, "Id", "Name");
            }

            SecurityPerson.GroupId = groupId;
            if (groupId != null)
            {
                SecurityPerson.TownId = townlist.FirstOrDefault(t => t.Groups.Any(u => u.Id == groupId))?.Id;
            }

            return Page();
        }
        [BindProperty]
        public string ReturnUrl { get; set; }
        [BindProperty]
        public SecurityPerson SecurityPerson { get; set; }


        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var townlist = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User));
                if (townlist.Any())
                {
                    var groups = (await _groupService.ListGroupsForTownEagerAsync(HttpContext.User, townlist.First().Id));
                    ViewData["GroupList"] = new SelectList(groups, "Id", "Name");
                }
               
                return Page();
            }

            _context.SecurityPersons.Add(SecurityPerson);
            await _context.SaveChangesAsync();

            return Redirect(Url.GetLocalUrl(ReturnUrl));
        }
        public async Task<bool> IsAdmin()
        {
            var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await admin).Succeeded;
        }
    }
}