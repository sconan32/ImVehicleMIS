using System;
using System.Collections.Generic;
using System.Text;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Infrastructure.Interfaces;

namespace Socona.ImVehicle.Infrastructure.Services
{
    public class DriverRepository : EfRepository<DriverItem>, IDriverRepository
    {
        public DriverRepository(VehicleDbContext dbContext) : base(dbContext)
        {

        }
    }
}
