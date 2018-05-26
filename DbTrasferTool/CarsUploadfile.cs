using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbTrasferTool
{
    [Table("Cars_uploadfile")]
    public partial class CarsUploadfile
    {
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("path", TypeName = "varchar(100)")]
        public string Path { get; set; }
        [Column("filename", TypeName = "varchar(1024)")]
        public string Filename { get; set; }
        [Required]
        [Column("date", TypeName = "date")]
        public string Date { get; set; }
        [Column("user_id")]
        public long? UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("CarsUploadfile")]
        public AuthUser User { get; set; }
    }
}
