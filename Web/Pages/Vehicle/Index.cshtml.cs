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

namespace Web.Pages.Vehicle
{
    public class IndexModel : PageModel
    {
        private readonly VehicleDbContext _dbContext;
        private readonly IAuthorizationService _authorizationService;
        private readonly ITownService _townService;
        public IndexModel(VehicleDbContext dbContext, IAuthorizationService authorizationService,ITownService townService)
        {
            _dbContext = dbContext;
            _authorizationService = authorizationService;
            _townService = townService;
        }

        public List<VehicleListViewModel> Vehicles { get; set; }

        public async Task<bool> CanEdit()
        {
            var tm = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireTownManagerRole");
            var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await tm).Succeeded || (await admin).Succeeded;
        }

        public async Task OnGetAsync()
        {
            //var townIdList = await _townService.GetAvailableTownIdsAsync(HttpContext.User);
            //var items = await _dbContext.Vehicles.Where(t=>townIdList.Contains(t.TownId??-1))
            //     .Include(t => t.Group).ThenInclude(g => g.Town)
            //     .Include(t => t.Driver)
            //     .ToListAsync();
            var items = new List<VehicleItem>();
            Vehicles = items.Select(t => new VehicleListViewModel(t)).ToList();
        }


    }
}
