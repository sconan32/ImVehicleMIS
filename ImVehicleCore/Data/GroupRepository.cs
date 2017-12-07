using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Socona.ImVehicle.Core.Data
{
    public class GroupRepository : EfRepository<GroupItem>, IGroupRepository
    {


        public GroupRepository(VehicleDbContext dbContext) : base(dbContext)
        {

        }


        public async Task<List<GroupItem>> GetGroupsOfTown(long townId)
        {
            return await _dbContext.Groups
                .Where(t => t.TownId == townId)
                .Include(t => t.Vehicles)
                .Include(t => t.Drivers)
                .Include(t => t.UserFiles)
                .Include(t => t.SecurityPersons)
                .Include(t => t.Town)
                .ToListAsync();
        }


        public async Task<List<GroupItem>> ListAllWithVehiclesAsync()
        {

            return await _dbContext.Groups
                .Include(t => t.Vehicles)
                .Include(t => t.Drivers)
                .Include(t => t.UserFiles)
                .Include(t => t.SecurityPersons)
                 .Include(t => t.Town)
                .ToListAsync();
        }
    }
}
