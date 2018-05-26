using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbTrasferTool
{
    [Table("auth_group_permissions")]
    public partial class AuthGroupPermissions
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("group_id")]
        public long GroupId { get; set; }
        [Column("permission_id")]
        public long PermissionId { get; set; }

        [ForeignKey("GroupId")]
        [InverseProperty("AuthGroupPermissions")]
        public AuthGroup Group { get; set; }
        [ForeignKey("PermissionId")]
        [InverseProperty("AuthGroupPermissions")]
        public AuthPermission Permission { get; set; }
    }
}
