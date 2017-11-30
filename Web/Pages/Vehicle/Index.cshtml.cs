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

namespace Web.Pages.Vehicle
{
    public class IndexModel : PageModel
    {
        private readonly VehicleDbContext _dbContext;

        public IndexModel(VehicleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<VehicleListViewModel> Vehicles { get; set; }

        public async Task OnGetAsync()
        {
           var    items= await _dbContext.Vehicles
                .Include(t => t.Group).ThenInclude(g => g.Town)
                .Include(t=>t.Driver)
                .ToListAsync();

            Vehicles = items.Select(t => new VehicleListViewModel()
            {
                Id = t.Id,
                License = t.LicenceNumber,
                Name = t.Name,
                Brand = t.Brand,
                Type = t.Type,
                Color = t.Color,
                LastRegisterDate = t.RegisterDate,
                GroupName = t.Group?.Name,
                TownName = t.Group?.Town?.Name,
                DriverName = t?.Driver?.Name,
                DriverTel = t?.Driver?.Tel,
            }).ToList();
        }

        
    }
}
