using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Socona.ImVehicle.Core.Data
{
    public class DriverItem : BaseEntity
    {
        [Display(Name = "驾驶证类型")]
        public string LicenseType { get; set; }

        [Display(Name = "电话")]
        [MaxLength(16)]
        public string Tel { get; set; }

        [Display(Name = "身份证号")]
        [MaxLength(32)]
        public string IdCardNumber { get; set; }

        [Display(Name = "性别")]
        public GenderType Gender { get; set; }

        [Display(Name = "驾驶证号")]
        [MaxLength(32)]
        public string LicenseNumber { get; set; }
        [Display(Name = "首次申领驾驶证于")]
        public DateTime? FirstLicenseIssueDate { get; set; }

        [Display(Name = "驾驶证签发日期")]
        public DateTime? LicenseIssueDate { get; set; }

        [Display(Name = "驾驶证有效年限")]
        public int? LicenseValidYears { get; set; }

        [Display(Name = "注册单位")]
        [MaxLength(256)]
        public string ContactAddress { get; set; }

       

        [Display(Name = "身份证国徽面")]
        [ForeignKey(nameof(IdCardImage1Id))]
        public virtual UserFileItem IdCardImage1 { get; set; }

        public long? IdCardImage1Id { get; set; }
        
        [Display(Name = "身份证照片面")]
        [ForeignKey(nameof(IdCardImage2Id))]
        public virtual UserFileItem IdCardImage2 { get; set; }

        public long? IdCardImage2Id { get; set; }
     


        [Display(Name = "驾驶证照片")]
        [ForeignKey(nameof(LicenseImageId))]
        public virtual UserFileItem LicenseImage { get; set; }
        
        public long? LicenseImageId { get; set; }     

     

        [Display(Name = "驾驶员头像")]
        [ForeignKey(nameof(AvatarImageId))]
        public virtual UserFileItem AvatarImage { get; set; }

        public long? AvatarImageId { get; set; }

        [Display(Name = "资质证书编号")]
        [MaxLength(32)]
        public string WarrantyCode { get; set; }
        public long? GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual GroupItem Group { get; set; }

        public long? TownId { get; set; }

        [ForeignKey("TownId")]
        public virtual TownItem Town { get; set; }

        [Display(Name = "住址")]
        [MaxLength(512)]
        public string LivingAddress { get; set; }

        [Display(Name = "职务")]
        [MaxLength(16)]
        public string Title { get; set; }

        [Display(Name = "户口类型")]
        public ResidentTypeEnum? ResidentType { get; set; }
      
        
        public long? ExtraImage1Id { get; set; }

        [Display(Name = "附加图片1")]
        [ForeignKey(nameof(ExtraImage1Id))]
        public virtual UserFileItem ExtraImage1 { get; set; }


        public long? ExtraImage2Id { get; set; }

        [Display(Name = "附加图片2")]
        [ForeignKey(nameof(ExtraImage2Id))]
        public virtual UserFileItem ExtraImage2 { get; set; }

        public long? ExtraImage3Id { get; set; }

        [Display(Name = "附加图片3")]
        [ForeignKey(nameof(ExtraImage3Id))]
        public virtual UserFileItem ExtraImage3 { get; set; }


        public virtual List<VehicleItem> Vehicles { get; set; }

        public bool IsValid()
        {
            var nowDate = DateTime.Now.Date;
            if (LicenseIssueDate == null)
            {
                return false;
            }
            return (LicenseIssueDate?.AddYears(LicenseValidYears ?? 0) > nowDate);
        }
    }
    public enum VehicleLicenseType
    {

        A1 = 1,
        A2 = 2,
        A3 = 3,
        B1 = 4,
        B2 = 5,
        C1 = 6,
        C2 = 7,
        C3 = 8,
        C4 = 9,
        C5 = 10,
        D = 11,
        E = 12,    
        F = 13,
        M = 14,
        N = 15,
        P = 16,
    }

    public enum GenderType
    {
        [Display(Name = "男")]
        Male = 1,
        [Display(Name = "女")]
        Female = 2,
    }

    public enum ResidentTypeEnum
    {
        [Display(Name = "本地")]
        Local = 1,
        [Display(Name = "异地")]
        NonLocal = 2,
    }
}
