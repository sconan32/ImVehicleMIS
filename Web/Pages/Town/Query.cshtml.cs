using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Web.ViewModels;
using Socona.ImVehicle.Core.Specifications;
using Socona.ImVehicle.Web.ViewModels.Specifications;
using Socona.ImVehicle.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace Web.Pages.Town
{
    public class QueryModel : PageModel
    {

        private readonly ITownService _townRepository;

        IGroupRepository _groupService;
        ISearchService _searchService;
        public QueryModel(ITownService townRepository, IGroupRepository groupService, ISearchService searchService)
        {
            _townRepository = townRepository;
            _groupService = groupService;
            _searchService = searchService;
        }

        public List<TownListViewModel> TownList { get; set; }



        public async Task OnGetAsync(string queryString)
        {

            ViewData["QueryString"] = queryString;
            var towns = await _townRepository.GetAvailableTownsEagerAsync(HttpContext.User);
            Expression expression;
            string url;
           // _searchService.BuildSearchExpression(":镇:"+queryString, out url, out expression);
            
            TownList = towns.OrderBy(t => t.Code).Select(t =>
          new TownListViewModel()
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
