using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ImVehicleCore.Data;
using Microsoft.AspNetCore.Http;

namespace Web.ViewModels
{
    public class VehicleViewModel
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "车牌号")]
        public string License { get; set; }

        [Display(Name = "类型")]
        public VehicleType Type { get; set; }

        [Display(Name = "使用性质")]
        public UsageType Usage { get; set; }
        [Display(Name = "品牌")]
        public string Brand { get; set; }
        [Display(Name = "型号")]
        public string Name { get; set; }

        [Display(Name = "颜色")]
        public string Color { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "上次注册时间")]
        public DateTime? LastRegisterDate { get; set; }
        [Display(Name = "安全单位")]
        public string GroupName { get; set; }
        [Display(Name = "街道")]
        public string TownName { get; set; }
        [Display(Name = "驾驶员")]
        public string DriverName { get; set; }
        [Display(Name = "驾驶员电话")]
        public string DriverTel { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "生产日期")]
        public DateTime? ProductionDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "报废日期")]
        public DateTime? DumpDate { get; set; }

        public bool IsDumpValid { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "强制保险日期")]
        public DateTime? InsuranceExpiredDate { get; set; }
        public bool IsInsuranceValid { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "登记日期")]
        public DateTime? RegisterDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "年检日期")]
        public DateTime? YearlyAuditDate { get; set; }
        public bool IsAuditValid { get; set; }

        [Display(Name = "车辆状态")]
        public string VehicleStatus { get; set; }

        [Display(Name = "备注")]
        public string Comment { get; set; }
        [Display(Name = "街道")]
        public long? TownId { get; set; }
        [Display(Name = "安全单位")]
        public long? GroupId { get; set; }
        [Display(Name = "驾驶员")]

        public long? DriverId { get; set; }

      
        [Display(Name = "车正面照片")]
        public IFormFile PhotoFront { get; set; }

        [Display(Name = "车正面照片")]
        public string PhotoFrontBase64 { get; set; }

        [Display(Name = "车背面照片")]
        public IFormFile PhotoRear { get; set; }

        [Display(Name = "车背面照片")]
        public string PhotoRearBase64 { get; set; }

        [Display(Name = "年检照片")]
        public IFormFile PhotoAudit { get; set; }

        [Display(Name = "年检照片")]
        public string PhotoAuditBase64 { get; set; }

        [Display(Name = "强制保险照片")]
        public IFormFile PhotoInsuarance { get; set; }

        
        [Display(Name = "强制保险照片")]
        public string PhotoInsuaranceBase64 { get; set; }

        [Display(Name = "挂靠单位")]

        public string Agent { get; set; }
        [Display(Name = "行驶证照片")]

        public IFormFile PhotoLicense { get; set; }
        [Display(Name = "行驶证照片")]
        public string PhotoLicenseBase64 { get; set; }
        [Display(Name = "GPS照片")]
        public IFormFile PhotoGps { get; set; }
        [Display(Name = "GPS照片")]
        public string PhotoGpsBase64 { get; set; }

        [Display(Name = "实际车主")]
        public string RealOwner { get; set; }
        [Display(Name = "已装GPS")]
        public bool GpsEnabled { get; set; }
        public bool IsValid { get; set; }
    }
}
