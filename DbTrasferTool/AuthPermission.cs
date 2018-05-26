using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbTrasferTool
{
    [Table("auth_permission")]
    public partial class AuthPermission
    {
        public AuthPermission()
        {
            AuthGroupPermissions = new HashSet<AuthGroupPermissions>();
            AuthUserUserPermissions = new HashSet<AuthUserUserPermissions>();
        }

        [Column("id")]
        public long Id { get; set; }
        [Column("content_type_id")]
        public long ContentTypeId { get; set; }
        [Required]
        [Column("codename", TypeName = "varchar(100)")]
        public string Codename { get; set; }
        [Required]
        [Column("name", TypeName = "varchar(255)")]
        public string Name { get; set; }

        [ForeignKey("ContentTypeId")]
        [InverseProperty("AuthPermission")]
        public DjangoContentType ContentType { get; set; }
        [InverseProperty("Permission")]
        public ICollection<AuthGroupPermissions> AuthGroupPermissions { get; set; }
        [InverseProperty("Permission")]
        public ICollection<AuthUserUserPermissions> AuthUserUserPermissions { get; set; }
    }
}
