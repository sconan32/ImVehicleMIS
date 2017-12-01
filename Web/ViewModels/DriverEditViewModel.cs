using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ImVehicleCore.Data;
using Microsoft.AspNetCore.Http;

namespace Web.ViewModels
{
    public class DriverEditViewModel
    {

        public long Id { get; set; }


        [Required]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "电话")]
        public string Tel { get; set; }
       
        [Display(Name = "身份证号")]
        public string IdCardNumber { get; set; }

        [Display(Name = "驾驶证号")]
        public string License { get; set; }

        [Display(Name = "驾驶证类型")]
        public VehicleLicenseType LicenseType { get; set; }
        [Display(Name = "性别")]
        public GenderType Gender { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "发证时间")]
        public DateTime LicenseIssue { get; set; }
        [Display(Name = "有效期限")]
        public int ValidYears { get; set; }

        [Display(Name = "注册车辆数")]
        public int VehiclesRegistered { get; set; }


        [Display(Name = "身份证国徽面照片")]
        public IFormFile PhotoIdCard1 { get; set; }

        public string PhotoIdCard1Base64 { get; set; }

        [Display(Name = "身份证照片面照片")]
        public IFormFile PhotoIdCard2 { get; set; }


        public string PhotoIdCard2Base64 { get; set; }

        [Display(Name = "驾驶证照片")]
        public IFormFile PhotoDriverLicense { get; set; }
        public string PhotoDriverLicenseBase64 { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "首次申领驾驶证于")]
        public DateTime FirstLicenseIssueDate { get; set; }
        [Display(Name = "资质证书编号")]
        public string WarrantyCode { get; set; }

        [Display(Name = "资质证书照片")]
        public IFormFile PhotoWarranty { get; set; }

        public string PhotoWarrantyBase64 { get; set; }

        [Display(Name = "驾驶员照片")]
        public IFormFile PhotoAvatar { get; set; }

        public string PhotoAvatarBase64 { get; set; }
        [Display(Name = "街道")]
        public long? TownId { get; set; }
        [Display(Name = "安全单位")]
        public long? GroupId { get; set; }
        [Display(Name = "住址")]
        public string LivingAddress { get; set; }
        [Display(Name = "职务")]
        public string Title { get; set; }
    }
}
