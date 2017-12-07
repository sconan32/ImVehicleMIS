using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using Socona.ImVehicle.Web.ViewModels;

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



        public async Task OnGetAsync()
        {
            var towns = await _townRepository.GetAvailableTownsEagerAsync(HttpContext.User);

            TownList = towns.OrderBy(t => t.Code).Select(t =>
          new TownItemListViewModel()
          {
              Id = t.Id,
              Code = t.Code,
              Name = t.Name,
              GroupCount = t.Groups?.Count ?? 0,
              DriverCount = t.Drivers?.Count ?? 0,
              VehicleCount = t.Vehicles?.Count ?? 0,
              IsValid = t.IsValid(),
              StatusText = t.IsValid() ? "正常" : "预警",
          }).ToList();
        }
    }
}
