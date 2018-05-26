using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbTrasferTool
{
    [Table("auth_group")]
    public partial class AuthGroup
    {
        public AuthGroup()
        {
            AuthGroupPermissions = new HashSet<AuthGroupPermissions>();
            AuthUserGroups = new HashSet<AuthUserGroups>();
        }

        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("name", TypeName = "varchar(80)")]
        public string Name { get; set; }

        [InverseProperty("Group")]
        public ICollection<AuthGroupPermissions> AuthGroupPermissions { get; set; }
        [InverseProperty("Group")]
        public ICollection<AuthUserGroups> AuthUserGroups { get; set; }
    }
}
