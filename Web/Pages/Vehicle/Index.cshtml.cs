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

        public class VehicleListViewModel
        {
            public long Id { get; set; }
            [Display(Name = "车牌号")]
            public string License { get; set; }
            [Display(Name = "品牌")]
            public string Brand { get; set; }
            [Display(Name = "型号")]
            public string Name { get; set; }
            [Display(Name = "类型")]
            public VehicleType Type { get; set; }
            [Display(Name = "颜色")]
            public string Color { get; set; }
            [Display(Name = "注册时间")]
            public DateTime LastRegisterDate { get; set; }
            [Display(Name = "安全单位")]
            public string GroupName { get; set; }
            [Display(Name = "街道")]
            public string TownName { get; set; }
            [Display(Name = "驾驶员")]
            public string DriverName { get; set; }
            [Display(Name = "驾驶员电话")]
            public string DriverTel { get; set; }
        }
    }
}
