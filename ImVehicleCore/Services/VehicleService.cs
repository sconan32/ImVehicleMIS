using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;

namespace Socona.ImVehicle.Core.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IAsyncRepository<VehicleItem> _vehicleRespository;   
        private readonly IAppLogger<VehicleService> _logger;

        public VehicleService(IAsyncRepository<VehicleItem> vehicleRespository,         
       
          IAppLogger<VehicleService> logger)
        {
            _vehicleRespository = vehicleRespository;
           
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

        public async Task<List<VehicleItem>> ListRangeAsync(ISpecification<VehicleItem> specification, int start, int count)
        {
            return await _vehicleRespository.ListRangeAsync(specification, start, count);
        }
    }
}
