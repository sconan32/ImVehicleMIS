using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Data;
using Microsoft.AspNetCore.Identity;

namespace Socona.ImVehicle.Core.Specifications
{
    public class Driver4UserSpecification : BaseSpecification<DriverItem>
    {
        private Driver4UserSpecification(Expression<Func<DriverItem, bool>> expression) : base(expression)
        {
        }

        public async static Task<Driver4UserSpecification> CreateAsync(ClaimsPrincipal user, UserManager<VehicleUser> userManager)
        {
            var expression = await BuildCriteriaAsync(user, userManager);
            return new Driver4UserSpecification(expression);
        }
        private async static Task<Expression<Func<DriverItem, bool>>> BuildCriteriaAsync(ClaimsPrincipal user, UserManager<VehicleUser> userManager)
        {
            var vUser = await userManager.GetUserAsync(user);
            var isTownManager = await userManager.IsInRoleAsync(vUser, "TownManager");
            return t => vUser != null && (!isTownManager || vUser.TownId == t.TownId);
        }


    }
}
