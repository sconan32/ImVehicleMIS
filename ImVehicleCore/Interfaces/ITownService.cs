using System.Threading.Tasks;
using ImVehicleCore.Data;

namespace ImVehicleCore.Interfaces
{
    public interface ITownService
    {
        Task<TownItem> GetTownByUser(string userName);
    }
}