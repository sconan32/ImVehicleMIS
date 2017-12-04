using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImVehicleCore.Data;
using ImVehicleCore.Interfaces;
using Web.ViewModels;
using ImVehicleCore.Specifications;
using Web.ViewModels.Specifications;

namespace Web.Pages.Town
{
    public class QueryModel : PageModel
    {

        private readonly ITownService _townRepository;

        IGroupRepository _groupService;
        public QueryModel(ITownService townRepository, IGroupRepository groupService)
        {
            _townRepository = townRepository;
            _groupService = groupService;
        }

        public List<TownItemListViewModel> TownList { get; set; }



        public async Task OnGetAsync(string queryString)
        {

            ViewData["QueryString"] = queryString;
            var towns = await _townRepository.GetAvailableTownsEagerAsync(HttpContext.User);

            TownList = towns.OrderBy(t => t.Code).Select(t =>
          new TownItemListViewModel()
          {
              Id = t.Id,
              Code = t.Code,
              Name = t.Name,
              GroupCount = t.Groups.Count,
              DriverCount = t.Drivers.Count,
              StatusText = t.IsValid() ? "正常" : "预警",
            IsValid = t.IsValid()
          }).Where(new TownListVmQueryStringSpecification(queryString).Criteria.Compile()).ToList();
    }
}

}
