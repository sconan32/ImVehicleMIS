using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbTrasferTool
{
    [Table("django_admin_log")]
    public partial class DjangoAdminLog
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("object_id")]
        public string ObjectId { get; set; }
        [Required]
        [Column("object_repr", TypeName = "varchar(200)")]
        public string ObjectRepr { get; set; }
        [Column("action_flag", TypeName = "smallint unsigned")]
        public long ActionFlag { get; set; }
        [Required]
        [Column("change_message")]
        public string ChangeMessage { get; set; }
        [Column("content_type_id")]
        public long? ContentTypeId { get; set; }
        [Column("user_id")]
        public long UserId { get; set; }
        [Required]
        [Column("action_time", TypeName = "datetime")]
        public string ActionTime { get; set; }

        [ForeignKey("ContentTypeId")]
        [InverseProperty("DjangoAdminLog")]
        public DjangoContentType ContentType { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("DjangoAdminLog")]
        public AuthUser User { get; set; }
    }
}
