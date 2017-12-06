using System.Collections.Generic;
using System.Threading.Tasks;
using ImVehicleCore.Data;

namespace ImVehicleCore.Interfaces
{
    public interface IGroupRepository:IAsyncRepository<GroupItem>
    {
        Task<List<GroupItem>> GetGroupsOfTown(long townId);

         Task<List<GroupItem>> ListAllWithVehiclesAsync();
    }
}