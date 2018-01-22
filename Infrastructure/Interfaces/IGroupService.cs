using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Specifications;

namespace Socona.ImVehicle.Core.Interfaces
{
   public interface IGroupService
    {

        Task<List<GroupItem>> ListAwailableGroupEagerAsync(ClaimsPrincipal claim);
        Task<List<GroupItem>> ListGroupsForTownEagerAsync(ClaimsPrincipal claim, long townId);

        Task<List<GroupItem>> ListRangeAsync(ISpecification<GroupItem> canFetch, int start, int count);

        Task<List<GroupItem>> ListAsync(ISpecification<GroupItem> canFetch);
        Task<GroupItem> GetByIdAsync(long id);
    }
}
