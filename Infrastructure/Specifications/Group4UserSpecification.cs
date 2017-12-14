using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Socona.ImVehicle.Infrastructure.Specifications
{
    public class Group4UserSpecification : BaseSpecification<GroupItem>
    {
        public Group4UserSpecification(Expression<Func<GroupItem, bool>> criteria = null) : base(criteria)
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
            var isAdmins = await userManager.IsInRoleAsync(vUser, "Admins");
            if (isAdmins)
            {
                return t => true;
            }
            var isGlobalVisitor = await userManager.IsInRoleAsync(vUser, "GlobalVisitor");
            if (isGlobalVisitor)
            {
                return t => true;
            }
            var isTownManager = await userManager.IsInRoleAsync(vUser, "TownManager");
            if (isTownManager)
            {
                return t => vUser != null && vUser.TownId == t.TownId;
            }

            var isGroupManager = await userManager.IsInRoleAsync(vUser, "GroupManager");
            if (isGroupManager)
            {
                return t => vUser != null && vUser.GroupId == t.Id;
            }

            return t => false;
        }

    }
}
