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

namespace Web.Pages.Group
{
    public class IndexModel : PageModel
    {
        private readonly IGroupRepository _groupRepository;

        public IndexModel(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public List<GroupListViewModel> Groups { get; set; }


        public class GroupListViewModel
        {
            public long Id { get; set; }

            [Display(Name="编码")]
            public string Code { get; set; }
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
        public async Task OnGetAsync()
        {
            var gs = await _groupRepository.ListAllWithVehiclesAsync();
            Groups = gs.Select(t => new GroupListViewModel()
            {
                Id = t.Id,
                Name = t.Name,
                ChiefName = t.ChiefName,
                ChiefTel = t.ChiefTel,
                VehicleCount = t.Vehicles.Count,
                InvalidCount = t.Vehicles.Count(v => v.LastRegisterDate.Date.AddYears(1) < DateTime.Now.Date)
            }).ToList();
        }
    }
}
