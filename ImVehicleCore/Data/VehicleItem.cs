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
        [Display(Name = "车辆类型")]
        public VehicleType Type { get; set; }
        [Display(Name = "营运类型")]
        public  UsageType Usage { get; set; }

        [Display(Name = "车牌号")]
        public string LicenceNumber { get; set; }


        public string DriverName { get; set; }

        public string DriverTel { get; set; }

        public string Brand { get; set; }

        [Display(Name = "颜色")]

        public string Color { get; set; }


        [Display(Name = "生产日期")]
        public DateTime ProductionDate { get; set; }
        [Display(Name = "强制保险有效期")]
        public DateTime InsuranceExpiredDate { get; set; }
        [Display(Name = "注册日期")]
        public DateTime RegisterDate { get; set; }
        [Display(Name = "年检日期")]
        public DateTime YearlyAuditDate { get; set; }

        [Display(Name = "车辆状态")]
        public string VehicleStatus { get; set; }

        [Display(Name = "备注")]
        public string Comment { get; set; }

        public long? GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual GroupItem Group { get; set; }


        public long? DriverId { get; set; }

        [ForeignKey("DriverId")]
        public virtual DriverItem Driver { get; set; }
        [Display(Name = "车正面照片")]
        public byte[] PhotoFront { get; set; }

        [Display(Name = "车背面照片")]
        public byte[] PhotoRear { get; set; }

        [Display(Name = "年检照片")]
        public byte[] PhotoAudit { get; set; }


        [Display(Name = "强制保险照片")]
        public byte[] PhotoInsuarance { get; set; }


        [Display(Name = "实际车主")]
        public string RealOwner { get; set; }


    }

    public enum UsageType
    {
        [Display(Name = "非营运")]
        NonCommercial =0x10000,
        [Display(Name = "货运")]
        Freight =0x20000,
        [Display(Name = "危化品运输")]
        Danger =0x40000,
        [Display(Name = "客运")]
        Passenger =0x80000,
        [Display(Name = "救护")]
        Ambulance =0x100000,



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
