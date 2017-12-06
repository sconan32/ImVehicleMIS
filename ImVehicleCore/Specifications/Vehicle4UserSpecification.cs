
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ImVehicleCore.Data;
using Microsoft.AspNetCore.Identity;

namespace ImVehicleCore.Specifications
{
    public class Vehicle4UserSpecification : BaseSpecification<VehicleItem>
    {
        private Vehicle4UserSpecification(Expression<Func<VehicleItem, bool>> expression) : base(expression)
        {
        }

        public async static Task<Vehicle4UserSpecification> CreateAsync(ClaimsPrincipal user, UserManager<VehicleUser> userManager)
        {
            var expression = await BuildCriteriaAsync(user, userManager);
            return new Vehicle4UserSpecification(expression);
        }
        private async static Task<Expression<Func<VehicleItem, bool>>> BuildCriteriaAsync(ClaimsPrincipal user, UserManager<VehicleUser> userManager)
        {
            var vUser = await userManager.GetUserAsync(user);
            var isTownManager = await userManager.IsInRoleAsync(vUser, "TownManager");
            return t => vUser != null && (!isTownManager || vUser.TownId == t.TownId);
        }


    }
}