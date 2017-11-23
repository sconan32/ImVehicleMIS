using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImVehicleCore.Data;
using ImVehicleCore.Interfaces;
using ImVehicleCore.Specifications;
using Microsoft.EntityFrameworkCore;

namespace ImVehicleCore.Data
{
   public class GroupRepository :EfRepository<GroupItem>, IGroupRepository
    {
       

        public GroupRepository(VehicleDbContext dbContext) : base(dbContext)
        {

        }


        public async Task<List<GroupItem>> GetGroupsOfTown(long townId)
        {
            return await ListAsync(new GroupsInTownSpecification(townId));
        }


        public async Task<List<GroupItem>> ListAllWithVehiclesAsync()
        {

            return await _dbContext.Groups.Include(t => t.Vehicles).ToListAsync();
        }
    }
}
