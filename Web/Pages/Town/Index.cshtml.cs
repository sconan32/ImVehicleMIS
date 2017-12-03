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
    public class IndexModel : PageModel
    {
        private readonly ITownService _townRepository;

        IGroupRepository _groupService;
        public IndexModel(ITownService townRepository, IGroupRepository groupService)
        {
            _townRepository = townRepository;
            _groupService = groupService;
        }

        public List<TownItemListViewModel> TownList { get; set; }


        public class TownItemListViewModel
        {
            public long Id { get; set; }
            [Display(Name = "编号")]
            public int Code { get; set; }
            [Display(Name = "名称")]
            public string Name { get; set; }
            [Display(Name = "安全单位数量")]
            public int GroupCount { get; set; }

            public bool IsValid { get; set; }

            public int VehicleCount { get; set; }
            [Display(Name = "驾驶员数量")]
            public int DriverCount { get; set; }

        }
        public async Task OnGetAsync()
        {
            var towns = await _townRepository.GetAvailableTownsEagerAsync(HttpContext.User);

            TownList = towns.OrderBy(t => t.Code).Select(t =>
          new TownItemListViewModel()
          {
              Id = t.Id,
              Code = t.Code,
              Name = t.Name,
              GroupCount = t.Groups.Count,
              DriverCount = t.Drivers.Count,
              IsValid = t.IsValid()

          }).ToList();
        }
    }
}
