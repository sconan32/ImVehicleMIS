using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbTrasferTool
{
    [Table("Cars_group")]
    public partial class CarsGroup
    {
        public CarsGroup()
        {
            CarsCar = new HashSet<CarsCar>();
            CarsDriver = new HashSet<CarsDriver>();
        }

        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("code", TypeName = "varchar(64)")]
        public string Code { get; set; }
        [Required]
        [Column("name", TypeName = "varchar(512)")]
        public string Name { get; set; }
        [Column("guarantee", TypeName = "varchar(64)")]
        public string Guarantee { get; set; }
        [Column("rules", TypeName = "varchar(64)")]
        public string Rules { get; set; }
        [Required]
        [Column("official_name", TypeName = "varchar(64)")]
        public string OfficialName { get; set; }
        [Required]
        [Column("phone", TypeName = "varchar(64)")]
        public string Phone { get; set; }
        [Required]
        [Column("official_position", TypeName = "varchar(128)")]
        public string OfficialPosition { get; set; }
        [Required]
        [Column("introduce")]
        public string Introduce { get; set; }
        [Column("photo", TypeName = "varchar(128)")]
        public string Photo { get; set; }

        [InverseProperty("Group")]
        public ICollection<CarsCar> CarsCar { get; set; }
        [InverseProperty("Group")]
        public ICollection<CarsDriver> CarsDriver { get; set; }
    }
}
