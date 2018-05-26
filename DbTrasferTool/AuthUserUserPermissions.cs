using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbTrasferTool
{
    [Table("auth_user_user_permissions")]
    public partial class AuthUserUserPermissions
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("user_id")]
        public long UserId { get; set; }
        [Column("permission_id")]
        public long PermissionId { get; set; }

        [ForeignKey("PermissionId")]
        [InverseProperty("AuthUserUserPermissions")]
        public AuthPermission Permission { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("AuthUserUserPermissions")]
        public AuthUser User { get; set; }
    }
}
