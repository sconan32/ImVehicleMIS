using System.Collections.Generic;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Core.Interfaces
{
    public interface IGroupRepository:IAsyncRepository<GroupItem>
    {
        Task<List<GroupItem>> GetGroupsOfTown(long townId);

         Task<List<GroupItem>> ListAllWithVehiclesAsync();
    }
}