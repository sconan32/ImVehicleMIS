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
        [MaxLength(32)]
        public string LicenceNumber { get; set; }

        [MaxLength(32)]
        public string DriverName { get; set; }
        [MaxLength(32)]
        public string DriverTel { get; set; }
        [MaxLength(16)]
        public string Brand { get; set; }

        [Display(Name = "颜色")]
        [MaxLength(16)]
        public string Color { get; set; }
        [Display(Name = "首次注册日期")]
        public DateTime? FirstRegisterDate { get; set; }

        [Display(Name = "生产日期")]
        public DateTime? ProductionDate { get; set; }

        [Display(Name = "检验有效日期")]
        public DateTime? AuditExpiredDate { get; set; }

        [Display(Name = "上次注册日期")]
        public DateTime? LastRegisterDate { get; set; }

        [Display(Name = "年检日期")]
        public DateTime? YearlyAuditDate { get; set; }

        [Display(Name = "车辆状态")]
        [MaxLength(32)]
        public string VehicleStatus { get; set; }

        [Display(Name = "备注")]
        public string Comment { get; set; }


        public long? TownId { get; set; }
        [ForeignKey("TownId")]
        public virtual TownItem Town { get; set; }

        public long? GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual GroupItem Group { get; set; }
        public DateTime? DumpDate { get; set; }

        [Display(Name = "GPS图像")]
        [ForeignKey(nameof(GpsImageId))]
        public virtual UserFileItem GpsImage { get; set; }


        public long? GpsImageId { get; set; }

        public long? DriverId { get; set; }

        [ForeignKey("DriverId")]
        public virtual DriverItem Driver { get; set; }

        [Display(Name = "车正面照片")]
        [ForeignKey(nameof(FrontImageId))]
        public virtual UserFileItem FrontImage { get; set; }


        public long? FrontImageId { get; set; }


        [Display(Name = "车背面照片")]
        [ForeignKey(nameof(RearImageId))]
        public virtual UserFileItem RearImage { get; set; }


        public long? RearImageId { get; set; }


        [Display(Name = "行驶证照片")]
        [ForeignKey(nameof(LicenseImageId))]
        public virtual UserFileItem LicenseImage { get; set; }


        public long? LicenseImageId { get; set; }


     


        [Display(Name = "实际车主")]
        [MaxLength(128)]
        public string RealOwner { get; set; }

        [Display(Name = "挂靠单位")]
        [MaxLength(256)]
        public string Agent { get; set; }

        [Display(Name = "GPS服务")]
        public bool? GpsEnabled { get; set; }

        public bool IsValid()
        {
            var nowDate = DateTime.Now.Date;
            if (YearlyAuditDate == null || DumpDate == null)
            {
                return false;
            }
            if (YearlyAuditDate?.AddYears(1) >= nowDate && DumpDate >= nowDate)
            {
                return true;
            }

            return false;
        }


        [Display(Name = "附加图片1")]
        [ForeignKey(nameof(ExtraImage1Id))]
        public virtual UserFileItem ExtraImage1 { get; set; }

        public long? ExtraImage1Id { get; set; }

        [Display(Name = "附加图片2")]
        [ForeignKey(nameof(ExtraImage2Id))]
        public virtual UserFileItem ExtraImage2 { get; set; }
        public long? ExtraImage2Id { get; set; }


        [Display(Name = "附加图片3")]
        [ForeignKey(nameof(ExtraImage3Id))]
        public virtual UserFileItem ExtraImage3 { get; set; }
        public long? ExtraImage3Id { get; set; }

    }

    public enum UsageType
    {
        [Display(Name = "非营运")]
        NonCommercial = 0x00,
        [Display(Name = "营运")]
        Commercial = 0x10000,
        [Display(Name = "货运")]
        Freight = 0x20000,
        [Display(Name = "危化品运输")]
        Danger = 0x40000,
        [Display(Name = "客运")]
        Passenger = 0x80000,
        [Display(Name = "救护")]
        Ambulance = 0x100000,
        [Display(Name = "公路客运")]
        PublicPassenger = 0x200000,
        [Display(Name = "旅游客运")]
        TourPassenger = 0x400000,
        [Display(Name = "校车")]
        SchoolBus = 0x800000,
        [Display(Name = "网约车")]
        NetHired = 0x1000000,
        [Display(Name = "公交客运")]
        Bus = 0x200000,
    }

    public enum VehicleType
    {


        [Display(Name = "重型自卸货车")]
        HeavyAuto = 0x1,
        [Display(Name = "重型普通货车")]
        HeavyLorry = 0x2,
        [Display(Name = "重型半挂牵引车")]
        HeavyLorry2 = 0x3,
        [Display(Name = "危险品运输车")]
        DangerLorry = 0x4,
        [Display(Name = "中型普通货车")]
        MiddleLorry = 0x5,
        [Display(Name = "轻型普通货车")]
        SmallLorry = 0x6,
        [Display(Name = "微型货车")]
        TinyLorry = 0x7,
        [Display(Name = "重型运输车")]
        HeavyLorry3 = 0x8,


        [Display(Name = "中型专项作业车")]
        MiddleSpecial = 0x9,
        [Display(Name = "重型特殊结构货车")]
        HeavySpecial = 0x10,
        [Display(Name = "重型罐式货车")]
        HeavyCanned = 0x11,
        [Display(Name = "重型载货专项作业车")]
        HeavySpecialLorry = 0x12,

        [Display(Name = "轻型封闭货车")]
        SmallLocked = 0x13,


        [Display(Name = "大型普通客车")]
        BigBus = 0x20,
        [Display(Name = "旅游客运车")]
        TourBus = 0x30,
        [Display(Name = "中型普通客车")]
        MiddleBus = 0x40,
        [Display(Name = "小型普通客车")]
        SmallBus = 0x50,


        [Display(Name = "特殊车辆")]
        SpecialVehicle = 0x400,
        [Display(Name = "三轮车")]
        Tricycle = 0x1000,
        [Display(Name = "四轮农用车")]
        FarmVehicle = 0x2000,
        [Display(Name = "摩托车")]
        Motorbike = 0x4000,


        [Display(Name = "营运车辆")]
        CommercialVehicle = 0x100,


        [Display(Name = "私家车")]
        PrivateCar = 0x200,
        [Display(Name = "出租车")]
        Taxi = 0x300,

        [Display(Name = "其他车辆")]
        Other = 0x0,


    }


}
