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

namespace Web.Pages.Towns
{
    public class DetailsModel : PageModel
    {

        private readonly ITownRepository _townRepository;

        IGroupRepository _groupService;
        public DetailsModel(ITownRepository townRepository, IGroupRepository groupService)
        {
            _townRepository = townRepository;
            _groupService = groupService;
        }
    
        public TownItemDetailViewModel TownItem { get; set; }

        public class TownItemDetailViewModel
        {
            public long Id { get; set; }

            [Display(Name="名称")]
            public string Name { get; set; }
            [Display(Name = "安全单位数量")]
            public int GroupCount { get; set; }

            public int VehicleCount { get; set; }

            public int DriverCount { get; set; }
            [Display(Name = "    其中：处于安全状态")]
            public int ValidCount { get; set; }
            [Display(Name = "        处于危险状态")]
            public int InvalidCount { get; set; }

            public List<GroupListViewModel> Groups { get; set; }
        }

        public class GroupListViewModel
        {
            public long Id { get; set; }
            [Display(Name = "名称")]
            public string Name { get; set; }
            [Display(Name = "负责人")]
            public string ChiefName { get; set; }
            [Display(Name = "负责人电话")]
            public string ChiefTel { get; set; }
            [Display(Name = "注册车辆数")]
            public int VehicleCount { get; set; }
            [Display(Name = "过期车辆数")]
            public int InvalidCount { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var town= await _townRepository.GetByIdWithGroupsAndVehiclesAsync(id.Value);
            TownItem = new TownItemDetailViewModel()
            {
                Id = town.Id,
                Name = town.Name,
                GroupCount = town.Groups.Count,
                ValidCount = town.Groups.Count(t => t.Vehicles.Any(v => v.RegisterDate.Date.AddYears(1) >= DateTime.Now.Date)),
                InvalidCount = town.Groups.Count(t => t.Vehicles.Any(v => v.RegisterDate.Date.AddYears(1) < DateTime.Now.Date)),
                Groups = town.Groups.Select(t => new GroupListViewModel()
                {
                    Id = t.Id,
                    Name=t.Name,
                    ChiefName=t.ChiefName,
                    ChiefTel=t.ChiefTel,
                    VehicleCount=t.Vehicles.Count,
                    InvalidCount=t.Vehicles.Count(v => v.RegisterDate.Date.AddYears(1) < DateTime.Now.Date)
                }).ToList(),
            };

            if (TownItem == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
