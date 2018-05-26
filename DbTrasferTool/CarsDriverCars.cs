using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbTrasferTool
{
    [Table("Cars_driver_cars")]
    public partial class CarsDriverCars
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("driver_id")]
        public long DriverId { get; set; }
        [Column("car_id")]
        public long CarId { get; set; }

        [ForeignKey("CarId")]
        [InverseProperty("CarsDriverCars")]
        public CarsCar Car { get; set; }
        [ForeignKey("DriverId")]
        [InverseProperty("CarsDriverCars")]
        public CarsDriver Driver { get; set; }
    }
}
