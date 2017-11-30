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

namespace Web.Pages.Driver
{
    public class IndexModel : PageModel
    {
        private readonly VehicleDbContext _dbContext;

        public IndexModel(VehicleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<DriverListViewModel> Drivers { get; set; }


      

        public async Task OnGetAsync()
        {
            var items = await _dbContext.Drivers.Include(t=>t.Vehicles).ToListAsync();
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
                Tel=t.Tel,
            }).ToList();
        }
    }
}
