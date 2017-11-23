using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImVehicleCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImVehicleCore.Data
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
                    .ThenInclude(g=>g.Vehicles)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
