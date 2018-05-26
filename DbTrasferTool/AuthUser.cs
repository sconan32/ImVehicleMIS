using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbTrasferTool
{
    [Table("auth_user")]
    public partial class AuthUser
    {
        public AuthUser()
        {
            AuthUserGroups = new HashSet<AuthUserGroups>();
            AuthUserUserPermissions = new HashSet<AuthUserUserPermissions>();
            CarsUploadfile = new HashSet<CarsUploadfile>();
            DjangoAdminLog = new HashSet<DjangoAdminLog>();
        }

        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("password", TypeName = "varchar(128)")]
        public string Password { get; set; }
        [Column("last_login", TypeName = "datetime")]
        public string LastLogin { get; set; }
        [Required]
        [Column("is_superuser", TypeName = "bool")]
        public string IsSuperuser { get; set; }
        [Required]
        [Column("first_name", TypeName = "varchar(30)")]
        public string FirstName { get; set; }
        [Required]
        [Column("last_name", TypeName = "varchar(30)")]
        public string LastName { get; set; }
        [Required]
        [Column("email", TypeName = "varchar(254)")]
        public string Email { get; set; }
        [Required]
        [Column("is_staff", TypeName = "bool")]
        public string IsStaff { get; set; }
        [Required]
        [Column("is_active", TypeName = "bool")]
        public string IsActive { get; set; }
        [Required]
        [Column("date_joined", TypeName = "datetime")]
        public string DateJoined { get; set; }
        [Required]
        [Column("username", TypeName = "varchar(150)")]
        public string Username { get; set; }

        [InverseProperty("User")]
        public ICollection<AuthUserGroups> AuthUserGroups { get; set; }
        [InverseProperty("User")]
        public ICollection<AuthUserUserPermissions> AuthUserUserPermissions { get; set; }
        [InverseProperty("User")]
        public ICollection<CarsUploadfile> CarsUploadfile { get; set; }
        [InverseProperty("User")]
        public ICollection<DjangoAdminLog> DjangoAdminLog { get; set; }
    }
}
