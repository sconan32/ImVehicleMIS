using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Socona.ImVehicle.Core.Data
{
    public class VehicleItem : BaseEntity
    {
        [Display(Name = "车辆类型")]
        public VehicleType Type { get; set; }
        [Display(Name = "营运类型")]
        public UsageType Usage { get; set; }

        [Display(Name = "车牌号")]
        public string LicenceNumber { get; set; }


        public string DriverName { get; set; }

        public string DriverTel { get; set; }

        public string Brand { get; set; }

        [Display(Name = "颜色")]

        public string Color { get; set; }
        [Display(Name = "首次注册日期")]
        public DateTime? FirstRegisterDate { get; set; }

        [Display(Name = "生产日期")]
        public DateTime? ProductionDate { get; set; }

        [Display(Name = "强制保险日期")]
        public DateTime? InsuranceExpiredDate { get; set; }

        [Display(Name = "上次注册日期")]
        public DateTime? LastRegisterDate { get; set; }

        [Display(Name = "年检日期")]
        public DateTime? YearlyAuditDate { get; set; }

        [Display(Name = "车辆状态")]
        public string VehicleStatus { get; set; }

        [Display(Name = "备注")]
        public string Comment { get; set; }


        public long? TownId { get; set; }

        public TownItem Town { get; set; }

        public long? GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual GroupItem Group { get; set; }
        public DateTime? DumpDate { get; set; }
        public byte[] PhotoGps { get; set; }
        public long? DriverId { get; set; }

        [ForeignKey("DriverId")]
        public virtual DriverItem Driver { get; set; }
        [Display(Name = "车正面照片")]
        public byte[] PhotoFront { get; set; }

        [Display(Name = "车背面照片")]
        public byte[] PhotoRear { get; set; }

        [Display(Name = "行驶证照片")]
        public byte[] PhotoLicense { get; set; }

        [Display(Name = "年检照片")]
        public byte[] PhotoAudit { get; set; }


        [Display(Name = "强制保险照片")]
        public byte[] PhotoInsuarance { get; set; }


        [Display(Name = "实际车主")]
        public string RealOwner { get; set; }

        [Display(Name = "挂靠单位")]
        [MaxLength(128)]
        public string Agent { get; set; }

        [Display(Name = "GPS服务")]
        public bool? GpsEnabled { get; set; }

        public bool IsValid()
        {
            var nowDate = DateTime.Now.Date;
            if (InsuranceExpiredDate?.AddYears(1) <= nowDate)
            {
                return false;
            }
            if (YearlyAuditDate?.AddYears(1) <= nowDate)
            {
                return false;
            }
            if (LastRegisterDate?.AddYears(1) <= nowDate)
            {
                return false;
            }

            return true;
        }
    }

    public enum UsageType
    {
        [Display(Name = "非营运")]
        NonCommercial = 0x00,
        [Display(Name = "货运")]
        Freight = 0x20000,
        [Display(Name = "危化品运输")]
        Danger = 0x40000,
        [Display(Name = "客运")]
        Passenger = 0x80000,
        [Display(Name = "救护")]
        Ambulance = 0x100000,



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
        Other = 0x0,


    }


}
