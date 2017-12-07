using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Socona.ImVehicle.Web.ViewModels;
using Socona.ImVehicle.Web.ViewModels.Specifications;

namespace Web.Pages.Group
{
    public class QueryModel : PageModel
    {


        private readonly IGroupService _groupRepository;
        private readonly IAuthorizationService _authorizationService;

        public QueryModel(IGroupService groupRepository, IAuthorizationService authorizationService)
        {
            _groupRepository = groupRepository;
            _authorizationService = authorizationService;
        }

        public List<GroupListViewModel> Groups { get; set; }



        public async Task<bool> CanEdit()
        {
            var tm = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireTownManagerRole");
            var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await tm).Succeeded || (await admin).Succeeded;
        }
        public async Task<bool> IsAdmin()
        {
            var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await admin).Succeeded;
        }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task OnGetAsync(string queryString)
        {
            ViewData["QueryString"] = queryString;
            var gs = await _groupRepository.ListAwailableGroupEagerAsync(HttpContext.User);
            Groups = gs.Select(t => new GroupListViewModel()
            {
                Id = t.Id,
                Code = t.Code,
                TownName = t.Town?.Name,
                Type = t.Type,
                Address = t.Address,
                License = t.License,
                Name = t.Name,
                ChiefName = t.ChiefName,
                ChiefTel = t.ChiefTel,
                VehicleCount = t.Vehicles.Count,
                InvalidVehicleCount = t.Vehicles.Count(v => !v.IsValid())
            }).Where(new GroupListVmQueryStringSpecification(queryString).Criteria.Compile()).ToList();
        }



    }
}
