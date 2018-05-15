using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Data;
using Microsoft.AspNetCore.Http;

namespace Socona.ImVehicle.Web.ViewModels
{
    public class DriverEditViewModel
    {

        public DriverEditViewModel() { }

        public DriverEditViewModel(DriverItem driver )
        {

            Id = driver.Id;
            Name = driver.Name;
            Gender = driver.Gender;
            FirstLicenseIssueDate = driver.FirstLicenseIssueDate;
            LicenseIssue = driver.LicenseIssueDate;
            ResidentType = driver.ResidentType;


            IdCardNumber = driver.IdCardNumber;
            License = driver.LicenseNumber;
            LicenseType = driver.LicenseType;
            ValidYears = driver.LicenseValidYears;
            LivingAddress = driver.LivingAddress;
            Tel = driver.Tel;
            Title = driver.Title;
            WarrantyCode = driver.WarrantyCode;

            TownId = driver.TownId;
            GroupId = driver.GroupId;

            PhotoDriverLicenseBase64 = driver.PhotoDriverLicense != null ? Convert.ToBase64String(driver.PhotoDriverLicense) : "";
            PhotoIdCard1Base64 = driver.PhotoIdCard1 != null ? Convert.ToBase64String(driver.PhotoIdCard1) : "";
            PhotoIdCard2Base64 = driver.PhotoIdCard2 != null ? Convert.ToBase64String(driver.PhotoIdCard2) : "";
            PhotoWarrantyBase64 = driver.PhotoWarranty != null ? Convert.ToBase64String(driver.PhotoWarranty) : "";

        }
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
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "发证时间")]
        public DateTime? LicenseIssue { get; set; }
        [Required]
        [Display(Name = "有效年限")]
        public int? ValidYears { get; set; }

        [Display(Name = "注册车辆数")]
        public int VehiclesRegistered { get; set; }


        [Display(Name = "附加照片1")]
        public IFormFile ExtraPhoto1 { get; set; }

        public string ExtraPhoto1Base64 { get; set; }

        public IFormFile PhotoIdCard1 { get; set; }

        public string PhotoIdCard1Base64 { get; set; }

        [Display(Name = "附加照片2")]
        public IFormFile ExtraPhoto2 { get; set; }

        public string ExtraPhoto2Base64 { get; set; }
        public IFormFile PhotoIdCard2 { get; set; }


        public string PhotoIdCard2Base64 { get; set; }

        [Display(Name = "驾驶证照片")]
        public IFormFile PhotoDriverLicense { get; set; }
        public string PhotoDriverLicenseBase64 { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "首次申领驾驶证于")]
        public DateTime? FirstLicenseIssueDate { get; set; }
        [Display(Name = "资质证书编号")]
        public string WarrantyCode { get; set; }

        [Display(Name = "附加照片3")]
        public IFormFile ExtraPhoto3 { get; set; }

        public string ExtraPhoto3Base64 { get; set; }
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

        [Display(Name = "户口属地")]
        public ResidentTypeEnum? ResidentType { get; set; }

    }
}
