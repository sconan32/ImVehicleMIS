using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Core.Interfaces
{
    public interface ITownService
    {
        Task<List<TownItem>> GetAvailableTownsEagerAsync(ClaimsPrincipal user);

        Task<List<long>> GetAvailableTownIdsAsync(ClaimsPrincipal user);

        Task<List<TownItem>> ListForUser(ClaimsPrincipal user, ISpecification<TownItem> specification);

        Task<TownItem> GetByIdAsync(long id);
    }
}