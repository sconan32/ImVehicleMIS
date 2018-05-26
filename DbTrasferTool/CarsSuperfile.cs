using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbTrasferTool
{
    [Table("Cars_superfile")]
    public partial class CarsSuperfile
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("filename", TypeName = "varchar(128)")]
        public string Filename { get; set; }
        [Required]
        [Column("date", TypeName = "date")]
        public string Date { get; set; }
        [Required]
        [Column("path", TypeName = "varchar(100)")]
        public string Path { get; set; }
    }
}
