using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ImVehicleCore.Data
{
    public class DriverItem : BaseEntity
    {
        [Display(Name="驾驶证类型")]
        public VehicleLicenseType LicenseType { get; set; }

        [Display(Name = "电话")]
        public string Tel { get; set; }
        [Display(Name = "身份证号")]
        public string IdCardNumber { get; set; }

        [Display(Name = "性别")]
        public GenderType Gender { get; set; }

        [Display(Name = "驾驶证号")]
        public string LicenseNumber { get; set; }
        [Display(Name = "首次申领驾驶证日期")]
        public DateTime FirstLicenseIssueDate { get; set; }

        [Display(Name = "驾驶证签发日期")]
        public DateTime LicenseIssueDate { get; set; }
        [Display(Name = "驾驶证有效年限")]
        public int LicenseValidYears { get; set; }


        //public DateTime LicenseExpireDate { get; set; }



        public virtual List<VehicleItem> Vehicles { get; set; }

        [Display(Name = "身份证国徽面照片")]
        public byte[] PhotoIdCard1 { get; set; }
        [Display(Name = "身份证照片面照片")]
        public byte[] PhotoIdCard2 { get; set; }
        [Display(Name = "驾驶证照片")]
        public byte[] PhotoDriverLicense { get; set; }

        [Display(Name = "资质证书照片")]
        public byte[] PhotoWarranty { get; set; }

        [Display(Name = "驾驶员照片")]
        public byte[] PhotoAvatar{ get; set; }
        [Display(Name = "资质证书编号")]
        public string WarrantyCode { get; set; }
        public long? GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual GroupItem Group { get; set; }
        [Display(Name = "住址")]
        public string LivingAddress { get; set; }
        [Display(Name = "职务")]
        public string Title { get; set; }
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
}
