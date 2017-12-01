using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ImVehicleCore.Data;
using Microsoft.AspNetCore.Http;

namespace Web.ViewModels
{
    public class VihicleEditViewModel
    {
        public long Id { get; set; }

        [Display(Name = "车牌号")]
        public string License { get; set; }

        [Display(Name = "类型")]
        public VehicleType Type { get; set; }

        [Display(Name = "营运方式")]
        public UsageType Usage { get; set; }
        [Display(Name = "品牌")]
        public string Brand { get; set; }
        [Display(Name = "型号")]
        public string Name { get; set; }

        [Display(Name = "颜色")]
        public string Color { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "注册时间")]
        public DateTime LastRegisterDate { get; set; }
        [Display(Name = "安全单位")]
        public string GroupName { get; set; }
        [Display(Name = "街道")]
        public string TownName { get; set; }
        [Display(Name = "驾驶员")]
        public string DriverName { get; set; }
        [Display(Name = "驾驶员电话")]
        public string DriverTel { get; set; }

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


        public long? DriverId { get; set; }

      
        [Display(Name = "车正面照片")]
        public IFormFile PhotoFront { get; set; }

        [Display(Name = "车背面照片")]
        public IFormFile PhotoRear { get; set; }

        [Display(Name = "年检照片")]
        public IFormFile PhotoAudit { get; set; }


        [Display(Name = "强制保险照片")]
        public IFormFile PhotoInsuarance { get; set; }


        [Display(Name = "实际车主")]
        public string RealOwner { get; set; }
    }
}
