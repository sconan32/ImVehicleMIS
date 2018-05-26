using Socona.ImVehicle.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Socona.ImVehicle.Core.Data;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Socona.ImVehicle.Infrastructure.Services
{
    public class UserFileService : IUserFileService
    {
        private readonly VehicleDbContext _context;
        public UserFileService(VehicleDbContext context)
        {
            _context = context;
        }
        public Task<List<UserFileItem>> GetGlobalUserFilesAsync()
        {
            return _context.Files.Where(t => t.Visibility == VisibilityType.Global && t.Status!= StatusType.Deleted).ToListAsync();
        }

        public Task<List<UserFileItem>> GetUserFilesForGroupAsync(long groupId)
        {
            return _context.Files.Where(
                t => t.Visibility == VisibilityType.CurrentGroup 
                    && t.GroupId== groupId && t.Status != StatusType.Deleted).ToListAsync();
        }

        public Task<List<UserFileItem>> GetUserFilesForTownAsync(long townId)
        {
            return _context.Files.Where(
                t => t.Visibility == VisibilityType.CurrentTown
                    && t.TownId== townId && t.Status != StatusType.Deleted).ToListAsync();
        }
    }
}
