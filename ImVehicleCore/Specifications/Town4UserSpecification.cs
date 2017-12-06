using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ImVehicleCore.Data;
using ImVehicleCore.Specifications;
using Microsoft.AspNetCore.Identity;

namespace Web.ViewModels.Specifications
{
   public class Group4UserSpecification : BaseSpecification<GroupItem>
    {
        private Group4UserSpecification(Expression<Func<GroupItem, bool>> expression) : base(expression)
        {
        }

        public async static Task<Group4UserSpecification> CreateAsync(ClaimsPrincipal user, UserManager<VehicleUser> userManager)
        {
            var expression = await BuildCriteriaAsync(user, userManager);
            return new Group4UserSpecification(expression);
        }
        private async static Task<Expression<Func<GroupItem, bool>>> BuildCriteriaAsync(ClaimsPrincipal user, UserManager<VehicleUser> userManager)
        {
            var vUser = await userManager.GetUserAsync(user);
            var isTownManager = await userManager.IsInRoleAsync(vUser, "TownManager");
            return t => vUser != null && (!isTownManager || vUser.TownId == t.TownId);         
        }


    }
}