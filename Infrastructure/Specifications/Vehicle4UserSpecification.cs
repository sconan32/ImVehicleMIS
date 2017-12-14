
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Data;
using Microsoft.AspNetCore.Identity;

namespace Socona.ImVehicle.Core.Specifications
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
                return t => vUser != null && vUser.GroupId == t.GroupId;
            }
            return t => false;
        }


    }
}