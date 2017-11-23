using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ImVehicleCore.Data
{
    public static class VehicleUserSeed
    {


        public static async Task SeedAsync(UserManager<VehicleUser> userManager)
        {
            var defaultUser = new VehicleUser { UserName = "admin", Email = "admin@admin.com" };
            await userManager.CreateAsync(defaultUser, "Pass@word1");


        }
    }
}
