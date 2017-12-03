using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ImVehicleCore.Data;

namespace ImVehicleCore.Interfaces
{
    public interface ITownService
    {
        Task<List<TownItem>> GetAvailableTownsEagerAsync(ClaimsPrincipal user);

        Task<List<long>> GetAvailableTownIdsAsync(ClaimsPrincipal user);
    }
}