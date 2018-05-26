using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbTrasferTool
{
    [Table("Cars_car")]
    public partial class CarsCar
    {
        public CarsCar()
        {
            CarsDriverCars = new HashSet<CarsDriverCars>();
        }

        [Column("id")]
        public long Id { get; set; }
        [Column("plate", TypeName = "varchar(128)")]
        public string Plate { get; set; }
        [Column("type", TypeName = "varchar(128)")]
        public string Type { get; set; }
        [Column("brand", TypeName = "varchar(128)")]
        public string Brand { get; set; }
        [Column("color", TypeName = "varchar(128)")]
        public string Color { get; set; }
        [Column("property", TypeName = "varchar(128)")]
        public string Property { get; set; }
        [Column(TypeName = "varchar(32)")]
        public string _3g { get; set; }
        [Column("pro_time", TypeName = "date")]
        public DateTime? ProTime { get; set; }
        [Column("dump_time", TypeName = "date")]
        public DateTime? DumpTime { get; set; }
        [Column("validity_inspection", TypeName = "date")]
        public DateTime? ValidityInspection { get; set; }
        [Column("insurance_end", TypeName = "date")]
        public DateTime? InsuranceEnd { get; set; }
        [Column("first_registered", TypeName = "date")]
        public DateTime? FirstRegistered { get; set; }
        [Column("status", TypeName = "varchar(32)")]
        public string Status { get; set; }
        [Column("attach", TypeName = "varchar(32)")]
        public string Attach { get; set; }
        [Column("real_owner", TypeName = "varchar(32)")]
        public string RealOwner { get; set; }
        [Column("phone", TypeName = "varchar(32)")]
        public string Phone { get; set; }
        [Column("addition")]
        public string Addition { get; set; }
        [Column("regist_photo", TypeName = "varchar(32)")]
        public string RegistPhoto { get; set; }
        [Column("driving_photo", TypeName = "varchar(32)")]
        public string DrivingPhoto { get; set; }
        [Column("insurance_photo", TypeName = "varchar(32)")]
        public string InsurancePhoto { get; set; }
        [Column("service_photo", TypeName = "varchar(32)")]
        public string ServicePhoto { get; set; }
        [Column("gps_photo", TypeName = "varchar(32)")]
        public string GpsPhoto { get; set; }
        [Column("group_id")]
        public long? GroupId { get; set; }

        [ForeignKey("GroupId")]
        [InverseProperty("CarsCar")]
        public CarsGroup Group { get; set; }
        [InverseProperty("Car")]
        public ICollection<CarsDriverCars> CarsDriverCars { get; set; }
    }
}
