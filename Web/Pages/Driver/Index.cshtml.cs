using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImVehicleCore.Data;
using ImVehicleCore.Interfaces;
using System.ComponentModel.DataAnnotations;
using Web.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.Driver
{
    public class IndexModel : PageModel
    {
        private readonly VehicleDbContext _dbContext;
        private readonly IAuthorizationService _authorizationService;
        public IndexModel(VehicleDbContext dbContext, IAuthorizationService authorizationService)
        {
            _dbContext = dbContext;
            _authorizationService = authorizationService;
        }

        public List<DriverListViewModel> Drivers { get; set; }


        public async Task<bool> CanEdit()
        {
            var tm = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireTownManagerRole");
            var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await tm).Succeeded || (await admin).Succeeded;
        }

        public async Task OnGetAsync()
        {
            var items = await _dbContext.Drivers
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
                LicenseIssue = t.LicenseIssueDate,
                ValidYears = t.LicenseValidYears,
                Gender = t.Gender,
                VehiclesRegistered = t.Vehicles.Count,
                Tel = t.Tel,
                Title = t.Title,
                TownName = t.Town?.Name,
                GroupName = t.Group?.Name,
            }).ToList();
        }
    }
}
