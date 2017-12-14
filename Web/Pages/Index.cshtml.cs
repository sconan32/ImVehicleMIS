using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static ImVehicleMIS.Pages.Account.LoginModel;

namespace ImVehicleMIS.Pages
{
    public class IndexModel : PageModel
    {

        private readonly UserManager<VehicleUser> _userManager;
        public IndexModel( UserManager<VehicleUser> userManager)
        {
            this._userManager = userManager;
        }
      
       

        public async Task<IActionResult> OnGetAsync()
        {


            var claim = HttpContext.User;
            if (claim.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(claim);
                if (user != null)
                {
                    if (await _userManager.IsInRoleAsync(user, "TownManager"))
                    {
                        return RedirectToPage("/Town/Details",new { @id=user.TownId});
                    }
                    if (await _userManager.IsInRoleAsync(user, "GroupManager"))
                    {
                        return RedirectToPage("/Group/Details", new { @id = user.GroupId });
                    }
                    else
                    {
                        return RedirectToPage("/District/Details", new { @id = 1 });
                    }
                }
            }
            return RedirectToPage("/Account/Login");

        }
    }
}
