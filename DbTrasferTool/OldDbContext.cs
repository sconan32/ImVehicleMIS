using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DbTrasferTool
{
    public partial class OldDbContext : DbContext
    {
        public virtual DbSet<AuthGroup> AuthGroup { get; set; }
        public virtual DbSet<AuthGroupPermissions> AuthGroupPermissions { get; set; }
        public virtual DbSet<AuthPermission> AuthPermission { get; set; }
        public virtual DbSet<AuthUser> AuthUser { get; set; }
        public virtual DbSet<AuthUserGroups> AuthUserGroups { get; set; }
        public virtual DbSet<AuthUserUserPermissions> AuthUserUserPermissions { get; set; }
        public virtual DbSet<CarsCar> CarsCar { get; set; }
        public virtual DbSet<CarsDriver> CarsDriver { get; set; }
        public virtual DbSet<CarsDriverCars> CarsDriverCars { get; set; }
        public virtual DbSet<CarsGroup> CarsGroup { get; set; }
        public virtual DbSet<CarsSuperfile> CarsSuperfile { get; set; }
        public virtual DbSet<CarsUploadfile> CarsUploadfile { get; set; }
        public virtual DbSet<CarsWebsiteconfig> CarsWebsiteconfig { get; set; }
        public virtual DbSet<DjangoAdminLog> DjangoAdminLog { get; set; }
        public virtual DbSet<DjangoContentType> DjangoContentType { get; set; }
        public virtual DbSet<DjangoMigrations> DjangoMigrations { get; set; }
        public virtual DbSet<DjangoSession> DjangoSession { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlite(@"data source=E:\Projects\version4\db.sqlite3;");
                optionsBuilder.UseMySql("Server=210.30.97.227;Database=car;Uid=imvehicle;Pwd=ImVehicleDb@;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthGroup>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<AuthGroupPermissions>(entity =>
            {
                entity.HasIndex(e => e.GroupId)
                    .HasName("auth_group_permissions_0e939a4f");

                entity.HasIndex(e => e.PermissionId)
                    .HasName("auth_group_permissions_8373b171");

                entity.HasIndex(e => new { e.GroupId, e.PermissionId })
                    .HasName("auth_group_permissions_group_id_0cd325b0_uniq")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.AuthGroupPermissions)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.AuthGroupPermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AuthPermission>(entity =>
            {
                entity.HasIndex(e => e.ContentTypeId)
                    .HasName("auth_permission_417f1b1c");

                entity.HasIndex(e => new { e.ContentTypeId, e.Codename })
                    .HasName("auth_permission_content_type_id_01ab375a_uniq")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.ContentType)
                    .WithMany(p => p.AuthPermission)
                    .HasForeignKey(d => d.ContentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AuthUser>(entity =>
            {
                entity.HasIndex(e => e.Username)
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<AuthUserGroups>(entity =>
            {
                entity.HasIndex(e => e.GroupId)
                    .HasName("auth_user_groups_0e939a4f");

                entity.HasIndex(e => e.UserId)
                    .HasName("auth_user_groups_e8701ad4");

                entity.HasIndex(e => new { e.UserId, e.GroupId })
                    .HasName("auth_user_groups_user_id_94350c0c_uniq")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.AuthUserGroups)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AuthUserGroups)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AuthUserUserPermissions>(entity =>
            {
                entity.HasIndex(e => e.PermissionId)
                    .HasName("auth_user_user_permissions_8373b171");

                entity.HasIndex(e => e.UserId)
                    .HasName("auth_user_user_permissions_e8701ad4");

                entity.HasIndex(e => new { e.UserId, e.PermissionId })
                    .HasName("auth_user_user_permissions_user_id_14a6b632_uniq")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.AuthUserUserPermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AuthUserUserPermissions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CarsCar>(entity =>
            {
                entity.HasIndex(e => e.GroupId)
                    .HasName("Cars_car_0e939a4f");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<CarsDriver>(entity =>
            {
                entity.HasIndex(e => e.GroupId)
                    .HasName("Cars_driver_0e939a4f");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<CarsDriverCars>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.CarsDriverCars)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.CarsDriverCars)
                    .HasForeignKey(d => d.DriverId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CarsGroup>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<CarsSuperfile>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<CarsUploadfile>(entity =>
            {
                entity.HasIndex(e => e.UserId)
                    .HasName("Cars_uploadfile_e8701ad4");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<CarsWebsiteconfig>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<DjangoAdminLog>(entity =>
            {
                entity.HasIndex(e => e.ContentTypeId)
                    .HasName("django_admin_log_417f1b1c");

                entity.HasIndex(e => e.UserId)
                    .HasName("django_admin_log_e8701ad4");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DjangoAdminLog)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DjangoContentType>(entity =>
            {
                entity.HasIndex(e => new { e.AppLabel, e.Model })
                    .HasName("django_content_type_app_label_76bd3d3b_uniq")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<DjangoMigrations>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<DjangoSession>(entity =>
            {
                entity.HasIndex(e => e.ExpireDate)
                    .HasName("django_session_de54fa62");

                entity.Property(e => e.SessionKey).ValueGeneratedNever();
            });
        }
    }
}
