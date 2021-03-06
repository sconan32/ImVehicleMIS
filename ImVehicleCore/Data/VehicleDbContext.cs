using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Socona.ImVehicle.Core.Data
{
    public class VehicleDbContext : IdentityDbContext<VehicleUser,VehicleRole,string>
    {
        public VehicleDbContext(DbContextOptions<VehicleDbContext> options)
            : base(options)
        {

        }

      
        public DbSet<UserFileItem> Files { get; set; }

        public DbSet<DistrictItem> Districts { get; set; }
        public DbSet<TownItem> Towns { get; set; }

        public DbSet<GroupItem> Groups { get; set; }

        public DbSet<SecurityPerson> SecurityPersons { get; set; }

        public DbSet<VehicleItem> Vehicles { get; set; }

        public DbSet<DriverItem> Drivers { get; set; }

      
        public DbSet<NewsItem> Newses { get; set; }

        public DbSet<VehicleOperation> Operations { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<UserFileItem>(action =>
            {
                action.HasOne(u => u.Group).WithMany(g => g.UserFiles).HasForeignKey(u => u.GroupId).OnDelete(DeleteBehavior.SetNull);
            });
           

        }
    }
}
