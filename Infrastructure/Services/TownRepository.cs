using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Socona.ImVehicle.Core.Data
{
    public class TownRepository : EfRepository<TownItem>, ITownRepository
    {

        public TownRepository(VehicleDbContext dbContext) : base(dbContext)
        {

        }

        public TownItem GetByIdWithGroups(long id)
        {
            return _dbContext.Towns.Include(t => t.Groups).FirstOrDefault(t => t.Id == id);
        }

        public Task<TownItem> GetByIdWithGroupsAsync(long id)
        {
            return _dbContext.Towns.Include(t => t.Groups).FirstOrDefaultAsync(t => t.Id == id);
        }


        public Task<List<TownItem>> ListAllWithGroupAsync()
        {

            return _dbContext.Towns.Include(t => t.Groups).ToListAsync();
        }

        public Task<TownItem> GetByIdWithGroupsAndVehiclesAsync(long id)
        {

            return _dbContext.Towns
                .Include(t => t.Groups)
                    .ThenInclude(g => g.Vehicles)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public Task<List<TownItem>> ListAllEagerAsync()
        {

            return _dbContext.Towns
                .Include(t => t.Groups)
                    .ThenInclude(u => u.Vehicles)
                .Include(t => t.Groups).
                    ThenInclude(u => u.Drivers)
                .Include(t => t.Users)
                 .Include(t => t.Drivers)
                    .ThenInclude(d => d.Vehicles)
                .ToListAsync();
        }

        public Task<TownItem> GetByIdEagerAsync(long id)
        {
            return _dbContext.Towns.Include(t => t.Groups)
                    .ThenInclude(u => u.Vehicles)
                .Include(t => t.Groups).
                    ThenInclude(u => u.Drivers)
                .Include(t => t.Users)
                .Include(t => t.Drivers)
                    .ThenInclude(d => d.Vehicles)
                 .Include(t => t.Drivers)
                    .ThenInclude(d => d.Town)
                 .Include(t => t.Drivers)
                    .ThenInclude(d => d.Group)
                .Include(t => t.Vehicles)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
