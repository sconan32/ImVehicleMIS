using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbTrasferTool
{
    [Table("Cars_driver")]
    public partial class CarsDriver
    {
        public CarsDriver()
        {
            CarsDriverCars = new HashSet<CarsDriverCars>();
        }

        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("name", TypeName = "varchar(64)")]
        public string Name { get; set; }
        [Column("id_card", TypeName = "varchar(32)")]
        public string IdCard { get; set; }
        [Column("sex", TypeName = "varchar(16)")]
        public string Sex { get; set; }
        [Column("car_license", TypeName = "varchar(16)")]
        public string CarLicense { get; set; }
        [Column("license_start", TypeName = "date")]
        public DateTime? LicenseStart { get; set; }
        [Column("license_end", TypeName = "date")]
        public DateTime? LicenseEnd { get; set; }
        [Column("license_time")]
        public long? LicenseTime { get; set; }
        [Column("contact_address", TypeName = "varchar(512)")]
        public string ContactAddress { get; set; }
        [Column("live_address", TypeName = "varchar(512)")]
        public string LiveAddress { get; set; }
        [Column("original", TypeName = "varchar(128)")]
        public string Original { get; set; }
        [Column("phone", TypeName = "varchar(128)")]
        public string Phone { get; set; }
        [Column("status", TypeName = "varchar(32)")]
        public string Status { get; set; }
        [Column("id_photo", TypeName = "varchar(32)")]
        public string IdPhoto { get; set; }
        [Column("driver_photo", TypeName = "varchar(32)")]
        public string DriverPhoto { get; set; }
        [Column("title_photo", TypeName = "varchar(32)")]
        public string TitlePhoto { get; set; }
        [Column("group_id")]
        public long? GroupId { get; set; }

        [ForeignKey("GroupId")]
        [InverseProperty("CarsDriver")]
        public CarsGroup Group { get; set; }
        [InverseProperty("Driver")]
        public ICollection<CarsDriverCars> CarsDriverCars { get; set; }
    }
}
