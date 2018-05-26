using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbTrasferTool
{
    [Table("auth_user_groups")]
    public partial class AuthUserGroups
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("user_id")]
        public long UserId { get; set; }
        [Column("group_id")]
        public long GroupId { get; set; }

        [ForeignKey("GroupId")]
        [InverseProperty("AuthUserGroups")]
        public AuthGroup Group { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("AuthUserGroups")]
        public AuthUser User { get; set; }
    }
}
