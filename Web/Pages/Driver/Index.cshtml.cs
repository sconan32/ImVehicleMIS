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


        public class DriverListViewModel
        {
            public long Id { get; set; }

            [Display(Name="姓名")]
            public string Name { get; set; }

            [Display(Name="电话")]
            public string Tel { get; set; }

            [Display(Name = "身份证号")]
            public string IdCardNumber { get; set; }

            [Display(Name = "驾驶证号")]
            public string License { get; set; }

            [Display(Name = "驾驶证类型")]
            public VehicleLicenseType LicenseType { get; set; }
            [Display(Name = "性别")]
            public GenderType Gender { get; set; }

            [Display(Name = "发证时间")]
            public DateTime LicenseIssue { get; set; }
            [Display(Name = "有效期限")]
            public int ValidYears { get; set; }

            [Display(Name = "注册车辆数")]
            public int VehiclesRegistered { get; set; }



        }


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
