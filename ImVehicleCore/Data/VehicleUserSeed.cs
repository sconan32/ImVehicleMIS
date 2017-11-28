using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ImVehicleCore.Data
{
    public static class VehicleUserSeed
    {


        public static async Task SeedAsync(UserManager<VehicleUser> userManager, RoleManager<VehicleRole> roleManager)
        {
            
            var adminRole = new VehicleRole() { Name = "Admin", Visible = false };
            var globalRole = new VehicleRole() { Name = "GlobalVisitor", Visible = true, LocalName = "全局用户", };
            var townRole = new VehicleRole() { Name = "TownManager", Visible = true, LocalName = "街道管理员", };
            // Add the Description as an argument:
            await roleManager.CreateAsync(adminRole);
            await roleManager.CreateAsync(globalRole);
            await roleManager.CreateAsync(townRole);

            var defaultUser = new VehicleUser { UserName = "admin", Email = "admin@admin.com" };
            await userManager.CreateAsync(defaultUser, "Pass@word1");
            await userManager.AddToRoleAsync(defaultUser, "Admin");

            // While you're at it, change this to your own log-in:
            var gjz = new VehicleUser() { UserName = "gjz", Email = "gjz@gjz.com" };
            await userManager.CreateAsync(gjz, "GJZ@gjz1");
            await userManager.AddToRoleAsync(gjz, "GlobalVisitor");
            // Be careful here - you  will need to use a password which will 
            // be valid under the password rules for the application, 
            // or the process will abort:
            var zsz = new VehicleUser() { UserName = "zsz", Email = "zsz@zsz.com" };
            await userManager.CreateAsync(gjz, "ZSZ@zsz1");
            await userManager.AddToRoleAsync(gjz, "TownManager");


        }
    }
}
