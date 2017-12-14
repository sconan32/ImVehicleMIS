using Socona.ImVehicle.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Socona.ImVehicle.Infrastructure.Interfaces
{
   public interface IUserFileService
    {

        Task<List<UserFileItem>> GetGlobalUserFilesAsync();

        Task<List<UserFileItem>> GetUserFilesForTownAsync(long townId);

        Task<List<UserFileItem>> GetUserFilesForGroupAsync(long groupId);
    }
}
