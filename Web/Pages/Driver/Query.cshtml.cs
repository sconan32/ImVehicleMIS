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
using Socona.ImVehicle.Infrastructure.Interfaces;
using Socona.ImVehicle.Infrastructure.Tools;
using Socona.ImVehicle.Web.ViewModels;
using Socona.ImVehicle.Web.ViewModels.Specifications;

namespace Socona.ImVehicle.Web.Pages.Driver
{
    public class QueryModel : PageModel
    {
  
        private readonly IAuthorizationService _authorizationService;
        private readonly ITownService _townService;
        private readonly IGroupService _groupService;
        private readonly UserManager<VehicleUser> _userManager;
        private readonly IDriverRepository _driverRepository;

        public QueryModel(IDriverRepository driverRepository, IAuthorizationService authorizationService, ITownService townService,
            IGroupService groupService, UserManager<VehicleUser> userManager)
        {
            _driverRepository = driverRepository;
            _authorizationService = authorizationService;
            _townService = townService;
            _groupService = groupService;
            _userManager = userManager;
        }
        [BindProperty(SupportsGet = true)]
        public DriverFilterViewModel FilterModel { get; set; }
        public List<DriverListViewModel> Drivers { get; set; }

        public List<TownItem> Towns { get; set; }

        public List<GroupItem> Groups { get; set; }

        private bool? canEdit = null;

        public async Task<bool> CanEdit()
        {
            if (canEdit == null)
            {
                var tm = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireTownManagerRole");
                var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
                canEdit = (await tm).Succeeded || (await admin).Succeeded;
            }
            return canEdit ?? false;
        }

        public async Task<IActionResult> OnPostAsync(string queryString)
        {
            ViewData["QueryString"] = queryString;

            Towns = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User));
            Groups = (await _groupService.ListAwailableGroupEagerAsync(HttpContext.User));
            ViewData["TownList"] = new SelectList(Towns, "Id", "Name");
            ViewData["GroupList"] = new SelectList(Groups, "Id", "Name");

            ISpecification<DriverItem> canFetch = await Driver4UserSpecification.CreateAsync(HttpContext.User, _userManager);
            canFetch = canFetch.And(FilterModel.ToExpression());
            canFetch.Includes.Add(t => t.Vehicles);
            canFetch.Includes.Add(t => t.Town);
            canFetch.Includes.Add(t => t.Group);
           
            var items = (await _driverRepository.ListAsync(canFetch)).OrderBy(t=>t.IsValid());

            if (FilterModel.ExportExcel == true)
            {
                var sigStr = $"::{HttpContext.User.Identity.Name}::socona.imvehicle.driver.export?search::{DateTime.Now.ToString("yyyyMMdd.HHmmss")}";
                var stream = ExcelHelper.ExportDrivers(items.ToList(), sigStr);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"exported{DateTime.Now.ToString("yyyyMMdd.HHmmss")}.xlsx");
            }

            Drivers = items.Select(t => new DriverListViewModel(t)).ToList();

            return Page();
        }

        public async Task OnGetAsync(string queryString)
        {
            ViewData["QueryString"] = queryString;


            Towns = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User));
            Groups = (await _groupService.ListAwailableGroupEagerAsync(HttpContext.User));
            ViewData["TownList"] = new SelectList(Towns, "Id", "Name");
            ViewData["GroupList"] = new SelectList(Groups, "Id", "Name");

            ISpecification<DriverItem> canFetch = await Driver4UserSpecification.CreateAsync(HttpContext.User, _userManager);
           
            canFetch.Includes.Add(t => t.Vehicles);
            canFetch.Includes.Add(t => t.Town);
            canFetch.Includes.Add(t => t.Group);


            var items = (await _driverRepository.ListAsync(canFetch)).OrderBy(t => t.IsValid()); ;
            Drivers = items.Select(t => new DriverListViewModel(t)).Where(new DriverListVmQueryStringSpecification(queryString).Criteria.Compile()).ToList();
        }


    }

    public class DriverFilterViewModel
    {
        public string Name { get; set; }

        public string IdCard { get; set; }

        public string Tel { get; set; }

        public ModelStatus? Status { get; set; }


        public long? TownId { get; set; }

        public long? GroupId { get; set; }

        public bool? ExportExcel { get; set; }

        public Expression<Func<DriverItem, bool>> ToExpression()
        {
            List<Expression> exprs = new List<Expression>();
            MethodInfo contains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            ParameterExpression argParam = Expression.Parameter(typeof(DriverItem), "t");
            if (!string.IsNullOrWhiteSpace(Name))
            {
                var lhs = Expression.Property(argParam, nameof(Name));
                var rhs = Expression.Constant(Name);
                exprs.Add(Expression.Call(lhs, contains, rhs));
            }
            if (!string.IsNullOrWhiteSpace(IdCard))
            {
                var lhs = Expression.Property(argParam, nameof(IdCard));
                var rhs = Expression.Constant(IdCard);
                exprs.Add(Expression.Call(lhs, contains, rhs));
            }
            if (!string.IsNullOrWhiteSpace(Tel))
            {
                var lhs = Expression.Property(argParam, nameof(Tel));
                var rhs = Expression.Constant(Tel);
                exprs.Add(Expression.Call(lhs, contains, rhs));
            }
            if (Status!=null)
            {
                MethodInfo isValid = typeof(DriverItem).GetMethod("IsValid");
                if (Status == ModelStatus.Ok)

                { exprs.Add(Expression.Call(argParam, isValid)); }
                else if(Status== ModelStatus.Warning)
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
            if (GroupId != null)
            {
                var lhs = Expression.Property(argParam, nameof(GroupId));
                var rhs = Expression.Constant(GroupId, typeof(long?));
                exprs.Add(Expression.Equal(lhs, rhs));
            }
            if (exprs.Count > 0)
            {
                var retExpr = exprs[0];
                for (int i = 1; i < exprs.Count; i++)
                {
                    retExpr = Expression.AndAlso(retExpr, exprs[i]);
                }
                return Expression.Lambda<Func<DriverItem, bool>>(retExpr, argParam);
            }

            return t => true;
        }
    }
}
