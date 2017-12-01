using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ImVehicleCore.Data;

namespace ImVehicleCore.Interfaces
{
   public interface IGroupService
    {

        Task<List<GroupItem>> ListAwailableGroupEagerAsync(ClaimsPrincipal claim);
        Task<List<GroupItem>> ListGroupsForTownEagerAsync(ClaimsPrincipal claim, long townId);
    }
}
