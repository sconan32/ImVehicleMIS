using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbTrasferTool
{
    [Table("django_content_type")]
    public partial class DjangoContentType
    {
        public DjangoContentType()
        {
            AuthPermission = new HashSet<AuthPermission>();
            DjangoAdminLog = new HashSet<DjangoAdminLog>();
        }

        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("app_label", TypeName = "varchar(100)")]
        public string AppLabel { get; set; }
        [Required]
        [Column("model", TypeName = "varchar(100)")]
        public string Model { get; set; }

        [InverseProperty("ContentType")]
        public ICollection<AuthPermission> AuthPermission { get; set; }
        [InverseProperty("ContentType")]
        public ICollection<DjangoAdminLog> DjangoAdminLog { get; set; }
    }
}
