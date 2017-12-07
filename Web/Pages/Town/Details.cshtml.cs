using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using Web.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.Towns
{
    public class DetailsModel : PageModel
    {

        private readonly ITownRepository _townRepository;
        private readonly IAuthorizationService _authorizationService;

        IGroupRepository _groupService;
        public DetailsModel(ITownRepository townRepository, IGroupRepository groupService,IAuthorizationService authorizationService)
        {
            _townRepository = townRepository;
            _groupService = groupService;
            _authorizationService = authorizationService;
        }

        public TownDetailViewModel TownItem { get; set; }


        public async Task<bool> CanEdit()
        {
            var tm=  _authorizationService.AuthorizeAsync(HttpContext.User, "RequireTownManagerRole");
            var admin=  _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await tm).Succeeded || (await admin).Succeeded;
        }


        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var town = await _townRepository.GetByIdEagerAsync(id.Value);
            TownItem = new TownDetailViewModel(town);
           
            if (TownItem == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
