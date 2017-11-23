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
        private readonly ITownRepository _townRepository;

        IGroupRepository _groupService;
        public IndexModel(ITownRepository townRepository, IGroupRepository groupService)
        {
            _townRepository = townRepository;
            _groupService = groupService;
        }

        public List<TownItemListViewModel> TownList { get; set; }


        public class TownItemListViewModel
        {
            [Display(Name = "编号")]
            public long Id { get; set; }
            [Display(Name = "名称")]
            public string Name { get; set; }
            [Display(Name = "安全单位数量")]
            public int GroupCount { get; set; }


        }
        public async Task OnGetAsync()
        {
            var towns = await _townRepository.ListAllWithGroupAsync();

            TownList =  towns.Select(  t =>
           new TownItemListViewModel()
           {
               Id = t.Id,
               Name = t.Name,
               GroupCount = t.Groups.Count,
           }).ToList();
        }
    }
}
