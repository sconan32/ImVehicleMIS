using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Socona.ImVehicle.Core.Migrations
{
    public partial class testdriveritem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    BaseRoleId = table.Column<string>(maxLength: 128, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 128, nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    LocalName = table.Column<string>(maxLength: 128, nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(maxLength: 128, nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Visible = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_AspNetRoles_BaseRoleId",
                        column: x => x.BaseRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<string>(maxLength: 512, nullable: true),
                    CreateBy = table.Column<string>(maxLength: 128, nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    DivisionType = table.Column<int>(nullable: false),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(maxLength: 128, nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateBy = table.Column<string>(maxLength: 128, nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    Event = table.Column<int>(nullable: false),
                    IpAddr = table.Column<string>(maxLength: 128, nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(maxLength: 128, nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    NewData = table.Column<string>(nullable: true),
                    OldData = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Summary = table.Column<string>(maxLength: 512, nullable: true),
                    Url = table.Column<string>(maxLength: 512, nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Towns",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<string>(maxLength: 512, nullable: true),
                    Code = table.Column<int>(nullable: false),
                    CreateBy = table.Column<string>(maxLength: 128, nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    DirstrictId = table.Column<long>(nullable: false),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(maxLength: 128, nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Towns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Towns_Districts_DirstrictId",
                        column: x => x.DirstrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Company = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 128, nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    Depart = table.Column<string>(maxLength: 32, nullable: true),
                    DistrictId = table.Column<long>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    GroupId = table.Column<long>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(maxLength: 128, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    RealName = table.Column<string>(maxLength: 128, nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    Serial = table.Column<string>(maxLength: 128, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 32, nullable: true),
                    TownId = table.Column<long>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    Type = table.Column<string>(maxLength: 32, nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Towns_TownId",
                        column: x => x.TownId,
                        principalTable: "Towns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Agent = table.Column<string>(maxLength: 256, nullable: true),
                    AuditExpiredDate = table.Column<DateTime>(nullable: true),
                    Brand = table.Column<string>(maxLength: 16, nullable: true),
                    Color = table.Column<string>(maxLength: 16, nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 128, nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    DriverId = table.Column<long>(nullable: true),
                    DriverName = table.Column<string>(maxLength: 32, nullable: true),
                    DriverTel = table.Column<string>(maxLength: 32, nullable: true),
                    DumpDate = table.Column<DateTime>(nullable: true),
                    ExtraImage1Id = table.Column<long>(nullable: true),
                    ExtraImage2Id = table.Column<long>(nullable: true),
                    ExtraImage3Id = table.Column<long>(nullable: true),
                    FirstRegisterDate = table.Column<DateTime>(nullable: true),
                    FrontImageId = table.Column<long>(nullable: true),
                    GpsEnabled = table.Column<bool>(nullable: true),
                    GpsImageId = table.Column<long>(nullable: true),
                    GroupId = table.Column<long>(nullable: true),
                    LastRegisterDate = table.Column<DateTime>(nullable: true),
                    LicenceNumber = table.Column<string>(maxLength: 32, nullable: true),
                    LicenseImageId = table.Column<long>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(maxLength: 128, nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    ProductionDate = table.Column<DateTime>(nullable: true),
                    RealOwner = table.Column<string>(maxLength: 128, nullable: true),
                    RearImageId = table.Column<long>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    TownId = table.Column<long>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Usage = table.Column<int>(nullable: false),
                    VehicleStatus = table.Column<string>(maxLength: 32, nullable: true),
                    VersionNumber = table.Column<int>(nullable: false),
                    YearlyAuditDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Towns_TownId",
                        column: x => x.TownId,
                        principalTable: "Towns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AvatarImageId = table.Column<long>(nullable: true),
                    ContactAddress = table.Column<string>(maxLength: 256, nullable: true),
                    CreateBy = table.Column<string>(maxLength: 128, nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    ExtraImage1Id = table.Column<long>(nullable: true),
                    ExtraImage2Id = table.Column<long>(nullable: true),
                    ExtraImage3Id = table.Column<long>(nullable: true),
                    FirstLicenseIssueDate = table.Column<DateTime>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    GroupId = table.Column<long>(nullable: true),
                    IdCardImage1Id = table.Column<long>(nullable: true),
                    IdCardImage2Id = table.Column<long>(nullable: true),
                    IdCardNumber = table.Column<string>(maxLength: 32, nullable: true),
                    LicenseImageId = table.Column<long>(nullable: true),
                    LicenseIssueDate = table.Column<DateTime>(nullable: true),
                    LicenseNumber = table.Column<string>(maxLength: 32, nullable: true),
                    LicenseType = table.Column<string>(nullable: true),
                    LicenseValidYears = table.Column<int>(nullable: true),
                    LivingAddress = table.Column<string>(maxLength: 512, nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(maxLength: 128, nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    ResidentType = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Tel = table.Column<string>(maxLength: 16, nullable: true),
                    Title = table.Column<string>(maxLength: 16, nullable: true),
                    TownId = table.Column<long>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false),
                    WarrantyCode = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drivers_Towns_TownId",
                        column: x => x.TownId,
                        principalTable: "Towns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<string>(maxLength: 512, nullable: true),
                    ApplicationFileId = table.Column<long>(nullable: true),
                    AttachmentFilePath = table.Column<string>(maxLength: 512, nullable: true),
                    ChiefName = table.Column<string>(maxLength: 32, nullable: true),
                    ChiefTel = table.Column<string>(maxLength: 32, nullable: true),
                    ChiefTitle = table.Column<string>(maxLength: 32, nullable: true),
                    Code = table.Column<string>(maxLength: 32, nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 128, nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    DriverGuranteeFileId = table.Column<long>(nullable: true),
                    ExtraImage1Id = table.Column<long>(nullable: true),
                    ExtraImage2Id = table.Column<long>(nullable: true),
                    ExtraImage3Id = table.Column<long>(nullable: true),
                    GroupGuranteeFileId = table.Column<long>(nullable: true),
                    License = table.Column<string>(maxLength: 32, nullable: true),
                    LicenseImageId = table.Column<long>(nullable: true),
                    MainImageId = table.Column<long>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(maxLength: 128, nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    PoliceOffice = table.Column<string>(maxLength: 32, nullable: true),
                    Policeman = table.Column<string>(maxLength: 32, nullable: true),
                    RegisterAddress = table.Column<string>(maxLength: 512, nullable: true),
                    RuleFileId = table.Column<long>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    TownId = table.Column<long>(nullable: true),
                    Type = table.Column<string>(maxLength: 32, nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Towns_TownId",
                        column: x => x.TownId,
                        principalTable: "Towns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientPath = table.Column<string>(maxLength: 512, nullable: true),
                    ContentType = table.Column<string>(maxLength: 64, nullable: true),
                    CreateBy = table.Column<string>(maxLength: 128, nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    DownloadCount = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(maxLength: 128, nullable: true),
                    GroupId = table.Column<long>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(maxLength: 128, nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    ServerPath = table.Column<string>(maxLength: 512, nullable: true),
                    Size = table.Column<long>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    TownId = table.Column<long>(nullable: true),
                    Type = table.Column<string>(maxLength: 16, nullable: true),
                    VersionNumber = table.Column<int>(nullable: false),
                    Visibility = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Files_Towns_TownId",
                        column: x => x.TownId,
                        principalTable: "Towns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SecurityPersons",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<string>(maxLength: 512, nullable: true),
                    Company = table.Column<string>(maxLength: 256, nullable: true),
                    CreateBy = table.Column<string>(maxLength: 128, nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    GroupId = table.Column<long>(nullable: true),
                    IdCardNum = table.Column<string>(maxLength: 32, nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(maxLength: 128, nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    RegisterAddress = table.Column<string>(maxLength: 512, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Tel = table.Column<string>(maxLength: 32, nullable: true),
                    Title = table.Column<string>(maxLength: 32, nullable: true),
                    TownId = table.Column<long>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityPersons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecurityPersons_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SecurityPersons_Towns_TownId",
                        column: x => x.TownId,
                        principalTable: "Towns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Newses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Area = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: false),
                    CreateBy = table.Column<string>(maxLength: 128, nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    Excerpt = table.Column<string>(nullable: true),
                    ExpireDate = table.Column<DateTime>(nullable: true),
                    HasDateRange = table.Column<bool>(nullable: true),
                    ImageFileId = table.Column<long>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(maxLength: 128, nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    Order = table.Column<int>(nullable: true),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    Source = table.Column<string>(maxLength: 256, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 256, nullable: false),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Newses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Newses_Files_ImageFileId",
                        column: x => x.ImageFileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_BaseRoleId",
                table: "AspNetRoles",
                column: "BaseRoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DistrictId",
                table: "AspNetUsers",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GroupId",
                table: "AspNetUsers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TownId",
                table: "AspNetUsers",
                column: "TownId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_AvatarImageId",
                table: "Drivers",
                column: "AvatarImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_ExtraImage1Id",
                table: "Drivers",
                column: "ExtraImage1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_ExtraImage2Id",
                table: "Drivers",
                column: "ExtraImage2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_ExtraImage3Id",
                table: "Drivers",
                column: "ExtraImage3Id");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_GroupId",
                table: "Drivers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_IdCardImage1Id",
                table: "Drivers",
                column: "IdCardImage1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_IdCardImage2Id",
                table: "Drivers",
                column: "IdCardImage2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_LicenseImageId",
                table: "Drivers",
                column: "LicenseImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_TownId",
                table: "Drivers",
                column: "TownId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_GroupId",
                table: "Files",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_TownId",
                table: "Files",
                column: "TownId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ApplicationFileId",
                table: "Groups",
                column: "ApplicationFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_DriverGuranteeFileId",
                table: "Groups",
                column: "DriverGuranteeFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ExtraImage1Id",
                table: "Groups",
                column: "ExtraImage1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ExtraImage2Id",
                table: "Groups",
                column: "ExtraImage2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ExtraImage3Id",
                table: "Groups",
                column: "ExtraImage3Id");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_GroupGuranteeFileId",
                table: "Groups",
                column: "GroupGuranteeFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_LicenseImageId",
                table: "Groups",
                column: "LicenseImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_MainImageId",
                table: "Groups",
                column: "MainImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_RuleFileId",
                table: "Groups",
                column: "RuleFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_TownId",
                table: "Groups",
                column: "TownId");

            migrationBuilder.CreateIndex(
                name: "IX_Newses_ImageFileId",
                table: "Newses",
                column: "ImageFileId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityPersons_GroupId",
                table: "SecurityPersons",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityPersons_TownId",
                table: "SecurityPersons",
                column: "TownId");

            migrationBuilder.CreateIndex(
                name: "IX_Towns_DirstrictId",
                table: "Towns",
                column: "DirstrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_DriverId",
                table: "Vehicles",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ExtraImage1Id",
                table: "Vehicles",
                column: "ExtraImage1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ExtraImage2Id",
                table: "Vehicles",
                column: "ExtraImage2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ExtraImage3Id",
                table: "Vehicles",
                column: "ExtraImage3Id");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_FrontImageId",
                table: "Vehicles",
                column: "FrontImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_GpsImageId",
                table: "Vehicles",
                column: "GpsImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_GroupId",
                table: "Vehicles",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_LicenseImageId",
                table: "Vehicles",
                column: "LicenseImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_RearImageId",
                table: "Vehicles",
                column: "RearImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_TownId",
                table: "Vehicles",
                column: "TownId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Groups_GroupId",
                table: "AspNetUsers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Groups_GroupId",
                table: "Vehicles",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Files_ExtraImage1Id",
                table: "Vehicles",
                column: "ExtraImage1Id",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Files_ExtraImage2Id",
                table: "Vehicles",
                column: "ExtraImage2Id",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Files_ExtraImage3Id",
                table: "Vehicles",
                column: "ExtraImage3Id",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Files_FrontImageId",
                table: "Vehicles",
                column: "FrontImageId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Files_GpsImageId",
                table: "Vehicles",
                column: "GpsImageId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Files_LicenseImageId",
                table: "Vehicles",
                column: "LicenseImageId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Files_RearImageId",
                table: "Vehicles",
                column: "RearImageId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Drivers_DriverId",
                table: "Vehicles",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Groups_GroupId",
                table: "Drivers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Files_AvatarImageId",
                table: "Drivers",
                column: "AvatarImageId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Files_ExtraImage1Id",
                table: "Drivers",
                column: "ExtraImage1Id",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Files_ExtraImage2Id",
                table: "Drivers",
                column: "ExtraImage2Id",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Files_ExtraImage3Id",
                table: "Drivers",
                column: "ExtraImage3Id",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Files_IdCardImage1Id",
                table: "Drivers",
                column: "IdCardImage1Id",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Files_IdCardImage2Id",
                table: "Drivers",
                column: "IdCardImage2Id",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Files_LicenseImageId",
                table: "Drivers",
                column: "LicenseImageId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Files_ApplicationFileId",
                table: "Groups",
                column: "ApplicationFileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Files_DriverGuranteeFileId",
                table: "Groups",
                column: "DriverGuranteeFileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Files_ExtraImage1Id",
                table: "Groups",
                column: "ExtraImage1Id",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Files_ExtraImage2Id",
                table: "Groups",
                column: "ExtraImage2Id",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Files_ExtraImage3Id",
                table: "Groups",
                column: "ExtraImage3Id",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Files_GroupGuranteeFileId",
                table: "Groups",
                column: "GroupGuranteeFileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Files_LicenseImageId",
                table: "Groups",
                column: "LicenseImageId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Files_MainImageId",
                table: "Groups",
                column: "MainImageId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Files_RuleFileId",
                table: "Groups",
                column: "RuleFileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Towns_Districts_DirstrictId",
                table: "Towns");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Groups_GroupId",
                table: "Files");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Newses");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "SecurityPersons");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Towns");
        }
    }
}
