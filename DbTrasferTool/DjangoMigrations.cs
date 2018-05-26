using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbTrasferTool
{
    [Table("django_migrations")]
    public partial class DjangoMigrations
    {
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("app", TypeName = "varchar(255)")]
        public string App { get; set; }
        [Required]
        [Column("name", TypeName = "varchar(255)")]
        public string Name { get; set; }
        [Required]
        [Column("applied", TypeName = "datetime")]
        public string Applied { get; set; }
    }
}
