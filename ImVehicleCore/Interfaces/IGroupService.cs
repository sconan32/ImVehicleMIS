using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ImVehicleCore.Data;
using Web.ViewModels.Specifications;

namespace ImVehicleCore.Interfaces
{
   public interface IGroupService
    {

        Task<List<GroupItem>> ListAwailableGroupEagerAsync(ClaimsPrincipal claim);
        Task<List<GroupItem>> ListGroupsForTownEagerAsync(ClaimsPrincipal claim, long townId);

        Task<List<GroupItem>> ListRangeAsync(Group4UserSpecification canFetch, int start, int count);
        
    }
}
