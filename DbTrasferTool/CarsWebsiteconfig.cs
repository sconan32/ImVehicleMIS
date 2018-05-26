using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbTrasferTool
{
    [Table("Cars_websiteconfig")]
    public partial class CarsWebsiteconfig
    {
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("logo", TypeName = "varchar(100)")]
        public string Logo { get; set; }
        [Required]
        [Column("web_site_name", TypeName = "varchar(512)")]
        public string WebSiteName { get; set; }
    }
}
