
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Socona.ImVehicle.Core;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Infrastructure.Authorization
{
    public class EntityAuthorizationHandler
                : AuthorizationHandler<OperationAuthorizationRequirement, BaseEntity>
    {
        UserManager<VehicleUser> _userManager;

        public EntityAuthorizationHandler(UserManager<VehicleUser>
            userManager)
        {
            _userManager = userManager;
        }

        protected override async Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement,
                                   BaseEntity resource)
        {
            if (context.User == null || resource == null)
            {
                return;
            }

            // If we're not asking for CRUD permission, return.

            if (requirement.Name != Constants.CreateOperationName &&
                requirement.Name != Constants.ReadOperationName &&
                requirement.Name != Constants.UpdateOperationName &&
                requirement.Name != Constants.DeleteOperationName)
            {

                return;
            }

            if (await (await _userManager.GetUserAsync(context.User)).CanDoAsync(requirement.Name, resource, _userManager))
            {
                context.Succeed(requirement);
            }


            return;
        }
    }
}
