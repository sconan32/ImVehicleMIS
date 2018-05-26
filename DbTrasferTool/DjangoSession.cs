using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbTrasferTool
{
    [Table("django_session")]
    public partial class DjangoSession
    {
        [Key]
        [Column("session_key", TypeName = "varchar(40)")]
        public string SessionKey { get; set; }
        [Required]
        [Column("session_data")]
        public string SessionData { get; set; }
        [Required]
        [Column("expire_date", TypeName = "datetime")]
        public string ExpireDate { get; set; }
    }
}
