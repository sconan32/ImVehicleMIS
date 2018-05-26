using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Data;
using Microsoft.AspNetCore.Http;
using Socona.ImVehicle.Infrastructure.Extensions;

namespace Socona.ImVehicle.Web.ViewModels
{
    public class DriverViewModel
    {

        public DriverViewModel() { }

        public DriverViewModel(DriverItem driver )
        {
            OriginalModel = driver;
            if (driver != null)
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

                PhotoDriverLicenseBase64 = driver.LicenseImage.ToBase64String();
                PhotoIdCard1Base64 = driver.IdCardImage1.ToBase64String();
                PhotoIdCard2Base64 = driver.IdCardImage2.ToBase64String();
                PhotoAvatarBase64 = driver.AvatarImage.ToBase64String();
               

                           
                ExtraImage1Base64 = driver.ExtraImage1?.ToBase64String();
                ExtraImage2Base64 = driver.ExtraImage2?.ToBase64String();
                ExtraImage3Base64 = driver.ExtraImage3?.ToBase64String();

                IsValid = driver.IsValid();

                VehiclesRegistered = driver.Vehicles?.Count??0;
                TownName = driver.Town?.Name;
                GroupName = driver.Group?.Name;

             

    }
        }
        public DriverItem OriginalModel { get; set; }

        public long Id { get; set; }
        public bool IsValid { get; set; }

        [Required]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "电话")]
        public string Tel { get; set; }

        [Display(Name = "身份证号")]
        public string IdCardNumber { get; set; }

        [Display(Name = "驾驶证号")]
        public string License { get; set; }

        [Display(Name = "准驾车型")]
        public string LicenseType { get; set; }
        [Display(Name = "性别")]
        public GenderType Gender { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "发证时间")]
        public DateTime? LicenseIssue { get; set; }
        [Required]
        [Display(Name = "有效期限")]
        public int? ValidYears { get; set; }

        [Display(Name = "注册车辆数")]
        public int VehiclesRegistered { get; set; }


     

        public IFormFile PhotoIdCard1 { get; set; }

        public string PhotoIdCard1Base64 { get; set; }


        [Display(Name = "附加照片1")]
        public IFormFile ExtraImage1 { get; set; }
        [Display(Name = "附加照片1")]
        public string ExtraImage1Base64 { get; set; }

        [Display(Name = "附加照片2")]
        public IFormFile ExtraImage2 { get; set; }
        [Display(Name = "附加照片2")]
        public string ExtraImage2Base64 { get; set; }


        [Display(Name = "附加照片3")]
        public IFormFile ExtraImage3 { get; set; }
        [Display(Name = "附加照片3")]
        public string ExtraImage3Base64 { get; set; }
        public IFormFile PhotoIdCard2 { get; set; }


        public string PhotoIdCard2Base64 { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "驾驶证有效期")]
        public DateTime? LicenseExpiredDate { get { return LicenseIssue?.AddYears(ValidYears ?? 0); } }

        [Display(Name = "驾驶证照片")]
        public IFormFile PhotoDriverLicense { get; set; }
        [Display(Name = "驾驶证照片")]
        public string PhotoDriverLicenseBase64 { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "首次申领驾驶证于")]
        public DateTime? FirstLicenseIssueDate { get; set; }
        [Display(Name = "资质证书编号")]
        public string WarrantyCode { get; set; }

  
        public IFormFile PhotoWarranty { get; set; }

        public string PhotoWarrantyBase64 { get; set; }

        [Display(Name = "驾驶员照片")]
        public IFormFile PhotoAvatar { get; set; }

        public string PhotoAvatarBase64 { get; set; }
        [Display(Name = "街道")]
        public long? TownId { get; set; }
        [Display(Name = "安全单位")]
        public long? GroupId { get; set; }

        [Display(Name = "街道")]
        public string TownName { get; set; }
        [Display(Name = "安全单位")]
        public string GroupName { get; set; }
        [Display(Name = "住址")]
        public string LivingAddress { get; set; }
        [Display(Name = "职务")]
        public string Title { get; set; }

        [Display(Name = "户口属地")]
        public ResidentTypeEnum? ResidentType { get; set; }


        public async Task FillDriverItemAsync(DriverItem driver)
        {
            driver.Name = this.Name;
            driver.Gender = this.Gender;
            driver.FirstLicenseIssueDate = this.FirstLicenseIssueDate;
            driver.LicenseIssueDate = this.LicenseIssue;
            driver.ResidentType = this.ResidentType;


            driver.IdCardNumber = this.IdCardNumber;
            driver.LicenseNumber = this.License;
            driver.LicenseType = this.LicenseType;
            driver.LicenseValidYears = this.ValidYears;
            driver.LivingAddress = this.LivingAddress;
            driver.Tel = this.Tel;
            driver.Title = this.Title;
            driver.WarrantyCode = this.WarrantyCode;

            
            driver.TownId = this.TownId;
            driver.GroupId = this.GroupId;

            
        }

    }
}
