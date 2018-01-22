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
using Socona.ImVehicle.Core.Services;
using Socona.ImVehicle.Core.Specifications;
using Socona.ImVehicle.Infrastructure.Tools;
using Socona.ImVehicle.Web.ViewModels;
using Socona.ImVehicle.Web.ViewModels.Specifications;

namespace Socona.ImVehicle.Web.Pages.Vehicle
{
    public class QueryModel : PageModel
    {
        private readonly IVehicleService _vehicleService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ITownService _townService;
        private readonly IGroupService _groupService;
        private readonly UserManager<VehicleUser> _userManager;
        public QueryModel(IVehicleService vehicleService, IAuthorizationService authorizationService,
            ITownService townService, IGroupService groupService, UserManager<VehicleUser> userManager)
        {
            _vehicleService = vehicleService;
            _authorizationService = authorizationService;
            _townService = townService;
            _groupService = groupService;
            _userManager = userManager;
        }

        public List<VehicleListViewModel> Vehicles { get; set; }

        [BindProperty(SupportsGet = true)]
        public VehicleFilterViewModel FilterModel { get; set; }


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

            ISpecification<VehicleItem> canFetch = await Vehicle4UserSpecification.CreateAsync(HttpContext.User, _userManager);
            canFetch = canFetch.And(FilterModel.ToExpression());
            canFetch.Includes.Add(t => t.Driver);
            canFetch.Includes.Add(t => t.Town);
            canFetch.Includes.Add(t => t.Group);
            canFetch.OrderBy(t => t.IsValid());

            var items = await _vehicleService.ListAsync(canFetch);
            if(FilterModel.ExportExcel==true)
            {
                var sigStr = $"::{HttpContext.User.Identity.Name}::socona.imvehicle.vehicle.export?search::{DateTime.Now.ToString("yyyyMMdd.HHmmss")}";
                var stream =ExcelHelper.ExportVehicles(items,sigStr);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"exported{DateTime.Now.ToString("yyyyMMdd.HHmmss")}.xlsx");
            }


            Vehicles = items.Select(t => new VehicleListViewModel(t)).ToList();

            return Page();
        }
        public async Task OnGetAsync(string queryString)
        {
            ViewData["QueryString"] = queryString;
            ISpecification<VehicleItem> canFetch = await Vehicle4UserSpecification.CreateAsync(HttpContext.User, _userManager);
            canFetch.Includes.Add(t => t.Driver);
            canFetch.Includes.Add(t => t.Town);
            canFetch.Includes.Add(t => t.Group);
            canFetch.OrderBy(t => t.IsValid());

            var items = await _vehicleService.ListAsync(canFetch);

            Towns = (await _townService.GetAvailableTownsEagerAsync(HttpContext.User));
            Groups = (await _groupService.ListAwailableGroupEagerAsync(HttpContext.User));
            ViewData["TownList"] = new SelectList(Towns, "Id", "Name");
            ViewData["GroupList"] = new SelectList(Groups, "Id", "Name");

            Vehicles = items.Select(t => new VehicleListViewModel(t)).Where(new VehicleListVmQueryStringSpecification(queryString).Criteria.Compile()).ToList();
        }
        public class VehicleFilterViewModel
        {
            public string LicenseNumber { get; set; }

            public UsageType? Usage { get; set; }

            public VehicleType? Type { get; set; }

            public ModelStatus? Status { get; set; }

            public string DriverName { get; set; }

            public long? TownId { get; set; }

            public long? GroupId { get; set; }

            public bool? ExportExcel { get; set; }

            public Expression<Func<VehicleItem, bool>> ToExpression()
            {
                List<Expression> exprs = new List<Expression>();
                MethodInfo contains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                ParameterExpression argParam = Expression.Parameter(typeof(VehicleItem), "t");
                if (!string.IsNullOrWhiteSpace(LicenseNumber))
                {
                    var lhs = Expression.Property(argParam, nameof(LicenseNumber));
                    var rhs = Expression.Constant(LicenseNumber);
                    exprs.Add(Expression.Call(lhs, contains, rhs));
                }
                if (!string.IsNullOrWhiteSpace(DriverName))
                {
                    var lhs = Expression.Property(argParam, "Driver");
                    lhs = Expression.Property(lhs, "Name");
                    var rhs = Expression.Constant(DriverName);
                    exprs.Add(Expression.Call(lhs, contains, rhs));
                }
                if (Usage != null)
                {
                    var lhs = Expression.Property(argParam, nameof(Usage));
                    var rhs = Expression.Constant(Usage);
                    exprs.Add(Expression.Equal(lhs, rhs));
                }
                if (Type != null)
                {
                    var lhs = Expression.Property(argParam, nameof(Type));
                    var rhs = Expression.Constant(Type);
                    exprs.Add(Expression.Equal(lhs, rhs));
                }
                if (Status != null)
                {
                    MethodInfo isValid = typeof(VehicleItem).GetMethod("IsValid");
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
                    return Expression.Lambda<Func<VehicleItem, bool>>(retExpr, argParam);
                }

                return t => true;
            }
        }

    }
}
