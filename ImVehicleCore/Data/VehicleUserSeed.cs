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

            var adminRole = new VehicleRole() { Name = "Admins", Visible = false };
            var globalRole = new VehicleRole() { Name = "GlobalVisitor", Visible = true, LocalName = "全局用户", };
            var townRole = new VehicleRole() { Name = "TownManager", Visible = true, LocalName = "街道管理员", };
            // Add the Description as an argument:
            if (!await roleManager.RoleExistsAsync("Admins"))
            { await roleManager.CreateAsync(adminRole); }
            if (!await roleManager.RoleExistsAsync("GlobalVisitor"))
            {
                await roleManager.CreateAsync(globalRole);
            }
            if (!await roleManager.RoleExistsAsync("TownManager"))
            {
                await roleManager.CreateAsync(townRole);
            }




            var user1 = await userManager.FindByNameAsync("admin");
            if (user1 == null)
            {
                user1 = new VehicleUser { UserName = "admin", Email = "admin@admin.com" };
                await userManager.CreateAsync(user1, "Pass@word1");
            }
            if (!await userManager.IsInRoleAsync(user1, "Admins"))
            { await userManager.AddToRoleAsync(user1, "Admins"); }


            // While you're at it, change this to your own log-in:
            var gjz = await userManager.FindByNameAsync("gjz");
            if (gjz == null)
            {
                gjz = new VehicleUser() { UserName = "gjz", Email = "gjz@gjz.com" };
                await userManager.CreateAsync(gjz, "GJZ@gjz1");

            }
            if (!await userManager.IsInRoleAsync(gjz, "GlobalVisitor"))
            { await userManager.AddToRoleAsync(gjz, "GlobalVisitor"); }

            // Be careful here - you  will need to use a password which will 
            // be valid under the password rules for the application, 
            // or the process will abort:
            var zsz = await userManager.FindByNameAsync("zsz");
            if (zsz == null)
            {
                zsz = new VehicleUser() { UserName = "zsz", Email = "zsz@zsz.com", TownId = 1 };
                await userManager.CreateAsync(zsz, "ZSZ@zsz1");
                
            }
            if (!await userManager.IsInRoleAsync(zsz, "TownManager"))
            { await userManager.AddToRoleAsync(zsz, "TownManager"); }

            var dlw = await userManager.FindByNameAsync("dlw");
            if (dlw == null)
            {
                dlw = new VehicleUser() { UserName = "dlw", Email = "dlw@dlw.com", TownId = 12 };
                await userManager.CreateAsync(dlw, "DLW@dlw1");
               
            }
            if (!await userManager.IsInRoleAsync(dlw, "TownManager"))
            { await userManager.AddToRoleAsync(dlw, "TownManager"); }

        }
    }
}
