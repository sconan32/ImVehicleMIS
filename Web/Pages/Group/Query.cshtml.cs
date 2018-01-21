using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Core.Specifications;
using Socona.ImVehicle.Infrastructure.Specifications;
using Socona.ImVehicle.Infrastructure.Tools;
using Socona.ImVehicle.Web.ViewModels;
using Socona.ImVehicle.Web.ViewModels.Specifications;

namespace Socona.ImVehicle.Web.Pages.Group
{
    public class QueryModel : PageModel
    {
        private readonly VehicleDbContext _dbContext;
        private readonly ITownService _townService;
        private readonly IGroupService _groupRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<VehicleUser> _userManager;
        public QueryModel(IGroupService groupRepository, IAuthorizationService authorizationService,
             ITownService townService, VehicleDbContext dbContext, UserManager<VehicleUser> userManager)
        {
            _dbContext = dbContext;
            _groupRepository = groupRepository;
            _authorizationService = authorizationService;
            _townService = townService;
            _userManager = userManager;
        }

        public List<GroupListViewModel> Groups { get; set; }

        [BindProperty(SupportsGet = true)]
        public GroupFilterViewModel FilterModel { get; set; }


        public List<TownItem> Towns { get; set; }


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

        public async Task OnPostAsync(string queryString)
        {
            ViewData["QueryString"] = queryString;

            Towns = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User));
            ViewData["TownList"] = new SelectList(Towns, "Id", "Name");


            ISpecification<GroupItem> canFetch = await Group4UserSpecification.CreateAsync(HttpContext.User, _userManager);
            canFetch = canFetch.And(FilterModel.ToExpression());
            canFetch.Includes.Add(t => t.Drivers);
            canFetch.Includes.Add(t => t.Town);
            canFetch.Includes.Add(t => t.Vehicles);
            canFetch = canFetch.OrderBy(t => t.IsValid());

            var items = await _groupRepository.ListAsync(canFetch);
            Groups = items.Select(t => new GroupListViewModel(t)).ToList();
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


            Towns = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User));

            ViewData["TownList"] = new SelectList(Towns, "Id", "Name");
        }

        public class GroupFilterViewModel
        {
            public string Name { get; set; }

            public string Type { get; set; }

            public string ChiefName { get; set; }


            public string PoliceOffice { get; set; }

            public string Policeman { get; set; }

            public ModelStatus? Status { get; set; }



            public long? TownId { get; set; }




            public Expression<Func<GroupItem, bool>> ToExpression()
            {
                List<Expression> exprs = new List<Expression>();
                MethodInfo contains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                ParameterExpression argParam = Expression.Parameter(typeof(GroupItem), "t");
                if (!string.IsNullOrWhiteSpace(Name))
                {
                    var lhs = Expression.Property(argParam, nameof(Name));
                    var rhs = Expression.Constant(Name);
                    exprs.Add(Expression.Call(lhs, contains, rhs));
                }
                if (!string.IsNullOrWhiteSpace(Type))
                {
                    var lhs = Expression.Property(argParam, "Type");
                    var rhs = Expression.Constant(Type);
                    exprs.Add(Expression.Call(lhs, contains, rhs));
                }
                if (!string.IsNullOrWhiteSpace(ChiefName))
                {
                    var lhs = Expression.Property(argParam, nameof(ChiefName));
                    var rhs = Expression.Constant(ChiefName);
                    exprs.Add(Expression.Call(lhs, contains, rhs));
                }
                if (!string.IsNullOrWhiteSpace(PoliceOffice))
                {
                    var lhs = Expression.Property(argParam, nameof(PoliceOffice));
                    var rhs = Expression.Constant(PoliceOffice);
                    exprs.Add(Expression.Call(lhs, contains, rhs));
                }
                if (!string.IsNullOrWhiteSpace(Policeman))
                {
                    var lhs = Expression.Property(argParam, nameof(Policeman));
                    var rhs = Expression.Constant(Policeman);
                    exprs.Add(Expression.Call(lhs, contains, rhs));
                }
                if (Status != null)
                {
                    MethodInfo isValid = typeof(GroupItem).GetMethod("IsValid");
                    if (Status == ModelStatus.Ok)

                    { exprs.Add(Expression.Call(argParam, isValid)); }
                    else if (Status == ModelStatus.Warning)
                    {
                        exprs.Add(Expression.Not(Expression.Call(argParam, isValid)));
                    }
                }
                if (TownId != null)
                {
                    var lhs = Expression.Property(argParam, nameof(TownId));
                    var rhs = Expression.Constant(TownId, typeof(long?));
                    exprs.Add(Expression.Equal(lhs, rhs));
                }

                if (exprs.Count > 0)
                {
                    var retExpr = exprs[0];
                    for (int i = 1; i < exprs.Count; i++)
                    {
                        retExpr = Expression.AndAlso(retExpr, exprs[i]);
                    }
                    return Expression.Lambda<Func<GroupItem, bool>>(retExpr, argParam);
                }

                return t => true;
            }
        }

    }
}
