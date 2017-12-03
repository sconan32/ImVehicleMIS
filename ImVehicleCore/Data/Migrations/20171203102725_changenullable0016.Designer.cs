﻿// <auto-generated />
using ImVehicleCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace ImVehicleCore.Data.Migrations
{
    [DbContext(typeof(VehicleDbContext))]
    [Migration("20171203102725_changenullable0016")]
    partial class changenullable0016
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ImVehicleCore.Data.DistrictItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Metadata");

                    b.Property<DateTime?>("ModificationDate");

                    b.Property<string>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<int>("Status");

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("ImVehicleCore.Data.DriverItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime?>("FirstLicenseIssueDate");

                    b.Property<int>("Gender");

                    b.Property<long?>("GroupId");

                    b.Property<string>("IdCardNumber");

                    b.Property<DateTime?>("LicenseIssueDate");

                    b.Property<string>("LicenseNumber");

                    b.Property<int>("LicenseType");

                    b.Property<int?>("LicenseValidYears");

                    b.Property<string>("LivingAddress");

                    b.Property<string>("Metadata");

                    b.Property<DateTime?>("ModificationDate");

                    b.Property<string>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<byte[]>("PhotoAvatar");

                    b.Property<byte[]>("PhotoDriverLicense");

                    b.Property<byte[]>("PhotoIdCard1");

                    b.Property<byte[]>("PhotoIdCard2");

                    b.Property<byte[]>("PhotoWarranty");

                    b.Property<int>("Status");

                    b.Property<string>("Tel");

                    b.Property<string>("Title");

                    b.Property<long?>("TownId");

                    b.Property<int>("VersionNumber");

                    b.Property<string>("WarrantyCode");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("TownId");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("ImVehicleCore.Data.GroupItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("AttachmentFilePath");

                    b.Property<string>("ChiefName");

                    b.Property<string>("ChiefTel");

                    b.Property<string>("ChiefTitle");

                    b.Property<string>("Code");

                    b.Property<string>("Comment");

                    b.Property<string>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("License");

                    b.Property<string>("Metadata");

                    b.Property<DateTime?>("ModificationDate");

                    b.Property<string>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<byte[]>("PhotoMain");

                    b.Property<byte[]>("PhotoSecurity");

                    b.Property<byte[]>("PhotoWarranty");

                    b.Property<string>("RegisterAddress");

                    b.Property<int>("Status");

                    b.Property<long?>("TownId");

                    b.Property<string>("Type");

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.HasIndex("TownId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("ImVehicleCore.Data.NewsItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<string>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Excerpt");

                    b.Property<DateTime?>("ExpireDate");

                    b.Property<bool?>("HasDateRange");

                    b.Property<string>("Metadata");

                    b.Property<DateTime?>("ModificationDate");

                    b.Property<string>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.Property<DateTime>("PublishDate");

                    b.Property<string>("Source");

                    b.Property<int>("Status");

                    b.Property<string>("Title");

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.ToTable("Newses");
                });

            modelBuilder.Entity("ImVehicleCore.Data.SecurityPerson", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Company");

                    b.Property<string>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<long?>("GroupId");

                    b.Property<string>("IdCardNum");

                    b.Property<string>("Metadata");

                    b.Property<DateTime?>("ModificationDate");

                    b.Property<string>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<string>("RegisterAddress");

                    b.Property<int>("Status");

                    b.Property<string>("Tel");

                    b.Property<string>("Title");

                    b.Property<long?>("TownId");

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("TownId");

                    b.ToTable("SecurityPersons");
                });

            modelBuilder.Entity("ImVehicleCore.Data.TownItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<int>("Code");

                    b.Property<string>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<long>("DirstrictId");

                    b.Property<string>("Metadata");

                    b.Property<DateTime?>("ModificationDate");

                    b.Property<string>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<int>("Status");

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.HasIndex("DirstrictId");

                    b.ToTable("Towns");
                });

            modelBuilder.Entity("ImVehicleCore.Data.UserFileItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClientPath");

                    b.Property<string>("ContentType");

                    b.Property<string>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<int>("DownloadCount");

                    b.Property<string>("FileName");

                    b.Property<long?>("GroupId");

                    b.Property<string>("Metadata");

                    b.Property<DateTime?>("ModificationDate");

                    b.Property<string>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<string>("ServerPath");

                    b.Property<long>("Size");

                    b.Property<int>("Status");

                    b.Property<long?>("TownId");

                    b.Property<string>("Type");

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("TownId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("ImVehicleCore.Data.VehicleItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Brand");

                    b.Property<string>("Color");

                    b.Property<string>("Comment");

                    b.Property<string>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<long?>("DriverId");

                    b.Property<string>("DriverName");

                    b.Property<string>("DriverTel");

                    b.Property<DateTime?>("FirstRegisterDate");

                    b.Property<long?>("GroupId");

                    b.Property<DateTime?>("InsuranceExpiredDate");

                    b.Property<DateTime?>("LastRegisterDate");

                    b.Property<string>("LicenceNumber");

                    b.Property<string>("Metadata");

                    b.Property<DateTime?>("ModificationDate");

                    b.Property<string>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<byte[]>("PhotoAudit");

                    b.Property<byte[]>("PhotoFront");

                    b.Property<byte[]>("PhotoInsuarance");

                    b.Property<byte[]>("PhotoRear");

                    b.Property<DateTime?>("ProductionDate");

                    b.Property<string>("RealOwner");

                    b.Property<int>("Status");

                    b.Property<long?>("TownId");

                    b.Property<int>("Type");

                    b.Property<int>("Usage");

                    b.Property<string>("VehicleStatus");

                    b.Property<int>("VersionNumber");

                    b.Property<DateTime?>("YearlyAuditDate");

                    b.HasKey("Id");

                    b.HasIndex("DriverId");

                    b.HasIndex("GroupId");

                    b.HasIndex("TownId");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("ImVehicleCore.Data.VehicleRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BaseRoleId");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("LocalName");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.Property<bool>("Visible");

                    b.HasKey("Id");

                    b.HasIndex("BaseRoleId");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("ImVehicleCore.Data.VehicleUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Company");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Depart");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("RealName");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Serial");

                    b.Property<string>("Title");

                    b.Property<long?>("TownId");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("TownId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ImVehicleCore.Data.DriverItem", b =>
                {
                    b.HasOne("ImVehicleCore.Data.GroupItem", "Group")
                        .WithMany("Drivers")
                        .HasForeignKey("GroupId");

                    b.HasOne("ImVehicleCore.Data.TownItem", "Town")
                        .WithMany("Drivers")
                        .HasForeignKey("TownId");
                });

            modelBuilder.Entity("ImVehicleCore.Data.GroupItem", b =>
                {
                    b.HasOne("ImVehicleCore.Data.TownItem", "Town")
                        .WithMany("Groups")
                        .HasForeignKey("TownId");
                });

            modelBuilder.Entity("ImVehicleCore.Data.SecurityPerson", b =>
                {
                    b.HasOne("ImVehicleCore.Data.GroupItem", "Group")
                        .WithMany("SecurityPersons")
                        .HasForeignKey("GroupId");

                    b.HasOne("ImVehicleCore.Data.TownItem", "Town")
                        .WithMany()
                        .HasForeignKey("TownId");
                });

            modelBuilder.Entity("ImVehicleCore.Data.TownItem", b =>
                {
                    b.HasOne("ImVehicleCore.Data.DistrictItem", "District")
                        .WithMany()
                        .HasForeignKey("DirstrictId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImVehicleCore.Data.UserFileItem", b =>
                {
                    b.HasOne("ImVehicleCore.Data.GroupItem", "Group")
                        .WithMany("UserFiles")
                        .HasForeignKey("GroupId");

                    b.HasOne("ImVehicleCore.Data.TownItem", "Town")
                        .WithMany()
                        .HasForeignKey("TownId");
                });

            modelBuilder.Entity("ImVehicleCore.Data.VehicleItem", b =>
                {
                    b.HasOne("ImVehicleCore.Data.DriverItem", "Driver")
                        .WithMany("Vehicles")
                        .HasForeignKey("DriverId");

                    b.HasOne("ImVehicleCore.Data.GroupItem", "Group")
                        .WithMany("Vehicles")
                        .HasForeignKey("GroupId");

                    b.HasOne("ImVehicleCore.Data.TownItem", "Town")
                        .WithMany()
                        .HasForeignKey("TownId");
                });

            modelBuilder.Entity("ImVehicleCore.Data.VehicleRole", b =>
                {
                    b.HasOne("ImVehicleCore.Data.VehicleRole", "BaseRole")
                        .WithMany()
                        .HasForeignKey("BaseRoleId");
                });

            modelBuilder.Entity("ImVehicleCore.Data.VehicleUser", b =>
                {
                    b.HasOne("ImVehicleCore.Data.TownItem", "Town")
                        .WithMany("Users")
                        .HasForeignKey("TownId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("ImVehicleCore.Data.VehicleRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ImVehicleCore.Data.VehicleUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ImVehicleCore.Data.VehicleUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("ImVehicleCore.Data.VehicleRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImVehicleCore.Data.VehicleUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ImVehicleCore.Data.VehicleUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
