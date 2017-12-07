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

namespace Web.Pages.Driver
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

        public List<DriverListViewModel> Drivers { get; set; }


        public async Task<bool> CanEdit()
        {
            var tm = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireTownManagerRole");
            var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await tm).Succeeded || (await admin).Succeeded;
        }

        public async Task OnGetAsync(string queryString)
        {
            ViewData["QueryString"] = queryString;
            var townidlist = await _townService.GetAvailableTownIdsAsync(HttpContext.User);
            var items = await _dbContext.Drivers.Where(t=>townidlist.Contains(t.TownId??-1))
                .Include(t => t.Vehicles)
                .Include(t => t.Town)
                .Include(t => t.Group)
                .ToListAsync();
            Drivers = items.Select(t => new DriverListViewModel()
            {
                Id = t.Id,
                Name = t.Name,
                IdCardNumber = t.IdCardNumber,
                License = t.LicenseNumber,
                LicenseType = t.LicenseType,
                LicenseIssueDate = t.LicenseIssueDate,
                LicenseValidYears = t.LicenseValidYears,
                 FirstLicenseIssueDate=t.FirstLicenseIssueDate,
                Gender = t.Gender,
                VehiclesRegistered = t.Vehicles.Count,
                Tel = t.Tel,
                Title = t.Title,
                TownName = t.Town?.Name,
                GroupName = t.Group?.Name,
            }).Where(new DriverListVmQueryStringSpecification(queryString).Criteria.Compile()).ToList();
        }
    }
}
