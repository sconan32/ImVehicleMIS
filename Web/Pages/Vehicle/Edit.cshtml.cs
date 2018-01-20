using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Web.ViewModels;

namespace Socona.ImVehicle.Web.Pages.Vehicle
{
    public class EditModel : PageModel
    {
        private readonly VehicleDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly ITownService _townService;
        private readonly UserManager<VehicleUser> _userManager;
        private readonly IGroupService _groupService;
        public EditModel(VehicleDbContext context, IAuthorizationService authorizationService, ITownService townService, UserManager<VehicleUser> userManager,
            IGroupService groupService)
        {
            _context = context;
            _authorizationService = authorizationService;
            _townService = townService;
            _userManager = userManager;
            _groupService = groupService;

        }

        public async Task<IActionResult> OnGetAsync(long? id, string returnUrl)
        {
            ReturnUrl = returnUrl;
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(t => t.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }
            VehicleItem = new VehicleViewModel(vehicle);
           
            var townlist = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User));
            ViewData["TownList"] = new SelectList(townlist, "Id", "Name");
            if (townlist.Any())
            {
                var groups = (await _groupService.ListGroupsForTownEagerAsync(HttpContext.User, townlist.First().Id));
                ViewData["GroupList"] = new SelectList(groups, "Id", "Name");
            }
            
            if (VehicleItem.GroupId != null)
            {
                VehicleItem.TownId = townlist.FirstOrDefault(t => t.Groups.Any(u => u.Id == VehicleItem.GroupId))?.Id;
            }
            if (VehicleItem.DriverId != null)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var drivers = await (_context.Drivers.Where(t => t.TownId == user.TownId)).ToListAsync();
                ViewData["DriverList"] = new SelectList(drivers, "Id", "Name");
            }
            return Page();
        }

        [BindProperty]
        public VehicleViewModel VehicleItem { get; set; }
        [BindProperty]
        public string ReturnUrl { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var townlist = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User));

                ViewData["TownList"] = new SelectList(townlist, "Id", "Name");
                if (townlist.Any())
                {
                    var groups = (await _groupService.ListGroupsForTownEagerAsync(HttpContext.User, townlist.First().Id));
                    ViewData["GroupList"] = new SelectList(groups, "Id", "Name");
                }

                return Page();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);



            var vehicle = _context.Vehicles.FirstOrDefault(v => v.Id == VehicleItem.Id);
            if (vehicle == null)
            {
                return NotFound();
            }
            await VehicleItem.FillVehicleItem(vehicle);
           
            
            vehicle.ModifyBy  = user.Id;
            vehicle.ModificationDate  = DateTime.Now;
            vehicle.Status = StatusType.OK;
            vehicle.VersionNumber += 1;


            _context.Entry (vehicle).State = EntityState.Modified;
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
