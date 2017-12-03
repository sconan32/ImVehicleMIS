using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImVehicleCore.Data;
using ImVehicleCore.Interfaces;

namespace ImVehicleCore.Data
{
    public class VehicleService : IVehicleService
    {
        private readonly IAsyncRepository<VehicleItem> _vehicleRespository;
        private readonly IUriComposer _uriComposer;
        private readonly IAppLogger<VehicleService> _logger;

        public VehicleService(IAsyncRepository<VehicleItem> vehicleRespository,         
          IUriComposer uriComposer,
          IAppLogger<VehicleService> logger)
        {
            _vehicleRespository = vehicleRespository;
            _uriComposer = uriComposer;
            this._logger = logger;           
        }

        public async Task CreateVehicleAsync()
        {
            var vehicle = new VehicleItem() { };

            await _vehicleRespository.AddAsync(vehicle);
        }

        public Task AddItemToVihecle(int basketId, int catalogItemId, decimal price, int quantity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteVihecleAsync(int basketId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetVihecleItemCountAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task SetVihecle(int basketId, Dictionary<string, int> quantities)
        {
            throw new NotImplementedException();
        }

        public Task TransferVihecleAsync(string anonymousId, string userName)
        {
            throw new NotImplementedException();
        }
    }
}
