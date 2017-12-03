using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImVehicleCore.Interfaces
{
    public interface IVehicleService
    {
        Task<int> GetVihecleItemCountAsync(string userName);
        Task TransferVihecleAsync(string anonymousId, string userName);
        Task AddItemToVihecle(int basketId, int catalogItemId, decimal price, int quantity);
        Task SetVihecle(int basketId, Dictionary<string, int> quantities);
        Task DeleteVihecleAsync(int basketId);
    }
}
