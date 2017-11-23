using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImVehicleCore.Data
{
    public class VehicleItem : BaseEntity
    {

        public VehicleType Type { get; set; }

     
        public string LicenceNumber { get; set; }

        public string DriverName { get; set; }

        public string DriverTel { get; set; }

        public string Brand { get; set; }

       

        public string Color { get; set; }

        public DateTime ProductionDate { get; set; }
        public DateTime InsuranceExpiredDate { get; set; }

        public DateTime LastRegisterDate { get; set; }
        public string VehicleStatus { get; set; }

        public string Comment { get; set; }

        public long? GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual GroupItem Group { get; set; }


        public long? DriverId { get; set; }

        [ForeignKey("DriverId")]
        public virtual DriverItem Driver { get; set; }

        public byte[] PhotoFront { get; set; }

        public byte[] PhotoRear { get; set; }


        public byte[] PhotoDriver { get; set; }




    }

    public enum VehicleType
    {
        [Display(Name = "重型自卸货车")]
        HeavyAuto = 0x1,
        [Display(Name = "重型运输车")]
        HeavyLorry = 0x2,
        [Display(Name = "危险品运输车")]
        DangerLorry = 0x4,
        [Display(Name = "特殊车辆")]
        SpecialVehicle = 0x8,
        [Display(Name = "校车")]
        SchoolBus = 0x10,
        [Display(Name = "旅游客运车")]
        TourBus = 0x20,
        [Display(Name = "营运车辆")]
        CommercialVehicle = 0x100,
        [Display(Name = "私家车")]
        PrivateCar = 0x200,
        [Display(Name = "其他")]
        Other = 0x1000,


    }

   
}
