using System.Collections.Generic;
using System.Threading.Tasks;
using ImVehicleCore.Data;

namespace ImVehicleCore.Interfaces
{
   public interface ITownRepository:IRepository<TownItem>,IAsyncRepository<TownItem>
    {
        TownItem GetByIdWithGroups(long id);
        Task<TownItem> GetByIdWithGroupsAsync(long id);
        Task<List<TownItem>> ListAllWithGroupAsync();

         Task<TownItem> GetByIdWithGroupsAndVehiclesAsync(long id);

        Task<List<TownItem>> ListAllEagerAsync();

        Task<TownItem> GetByIdEagerAsync(long id);
    }
}