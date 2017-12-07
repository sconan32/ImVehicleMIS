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
using Socona.ImVehicle.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Socona.ImVehicle.Web.ViewModels.Specifications;

namespace Web.Pages.Vehicle
{
    public class QueryModel : PageModel
    {
        private readonly VehicleDbContext _dbContext;
        private readonly IAuthorizationService _authorizationService;
        private readonly ITownService _townService;
        public QueryModel(VehicleDbContext dbContext, IAuthorizationService authorizationService,ITownService townService)
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

        public async Task OnGetAsync(string queryString)
        {
            ViewData["QueryString"] = queryString;
            var townIdList = await _townService.GetAvailableTownIdsAsync(HttpContext.User);
            var items = await _dbContext.Vehicles.Where(t=>townIdList.Contains(t.TownId??-1))
                 .Include(t => t.Group).ThenInclude(g => g.Town)
                 .Include(t => t.Driver)
                 .ToListAsync();

            Vehicles = items.Select(t => new VehicleListViewModel(t)).Where(new VehicleListVmQueryStringSpecification(queryString).Criteria.Compile()).ToList();
        }


    }
}
