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
    public class VehicleViewModel
    {

        public VehicleViewModel() { }

        public VehicleViewModel(VehicleItem vehicle)
        {

            OriginalModel = vehicle;


            Id = vehicle.Id;
            Brand = vehicle.Brand;
            Color = vehicle.Color;
            Comment = vehicle.Comment;
            DriverId = vehicle.DriverId;
            GroupId = vehicle.GroupId;
            InsuranceExpiredDate = vehicle.InsuranceExpiredDate;
            LastRegisterDate = vehicle.LastRegisterDate;
            RegisterDate = vehicle.LastRegisterDate;
            License = vehicle.LicenceNumber;
            Name = vehicle.Name;
            ProductionDate = vehicle.ProductionDate;
            RealOwner = vehicle.RealOwner;
            Type = vehicle.Type;
            Usage = vehicle.Usage;
            VehicleStatus = vehicle.VehicleStatus;
            YearlyAuditDate = vehicle.YearlyAuditDate;
            Agent = vehicle.Agent;
            DumpDate = vehicle.DumpDate;
            GroupName = vehicle.Group?.Name;
            TownName = vehicle.Town?.Name;
            DriverName = vehicle.Driver?.Name;
            DriverTel = vehicle.Driver?.Tel;

            PhotoLicenseBase64 = vehicle.PhotoLicense.ToBase64String();
            PhotoGpsBase64 = vehicle.PhotoGps.ToBase64String();
            PhotoAuditBase64 = vehicle.PhotoAudit.ToBase64String();
            PhotoFrontBase64 = vehicle.PhotoFront.ToBase64String();
            PhotoRearBase64 = vehicle.PhotoRear.ToBase64String();
            PhotoInsuaranceBase64 = vehicle.PhotoInsuarance.ToBase64String();
            ExtraPhoto1Base64 = vehicle.ExtraPhoto1.ToBase64String();
            ExtraPhoto2Base64 = vehicle.ExtraPhoto1.ToBase64String();
            ExtraPhoto3Base64 = vehicle.ExtraPhoto1.ToBase64String();
        }

        public VehicleItem OriginalModel { get; set; }

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

        [Display(Name = "附加照片3")]
        public IFormFile PhotoAudit { get; set; }

        [Display(Name = "附加照片3")]
        public string PhotoAuditBase64 { get; set; }

        [Display(Name = "附加照片1")]
        public IFormFile PhotoInsuarance { get; set; }


        [Display(Name = "附加照片1")]
        public string PhotoInsuaranceBase64 { get; set; }

        [Display(Name = "挂靠单位")]

        public string Agent { get; set; }
        [Display(Name = "附加照片2")]

        public IFormFile PhotoLicense { get; set; }
        [Display(Name = "附加照片2")]
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


        [Display(Name = "附加照片1")]
        public IFormFile ExtraPhoto1 { get; set; }
        public string ExtraPhoto1Base64 { get; private set; }

        [Display(Name = "附加照片2")]
        public IFormFile ExtraPhoto2 { get; set; }
        public string ExtraPhoto2Base64 { get; private set; }

        [Display(Name = "附加照片3")]
        public IFormFile ExtraPhoto3 { get; set; }
        public string ExtraPhoto3Base64 { get; private set; }


        public async Task FillVehicleItem(VehicleItem vehicle)
        {
            vehicle.Name = this.Name;
            vehicle.Brand = this.Brand;
            vehicle.Color = this.Color;
            vehicle.Comment = this.Comment;
            vehicle.InsuranceExpiredDate = this.InsuranceExpiredDate;
            vehicle.LicenceNumber = this.License;
            vehicle.DumpDate = this.DumpDate;
            vehicle.ProductionDate = this.ProductionDate;
            vehicle.RealOwner = this.RealOwner;
            vehicle.LastRegisterDate = this.RegisterDate;
            vehicle.Type = this.Type;
            vehicle.Usage = this.Usage;
            vehicle.YearlyAuditDate = this.YearlyAuditDate;
            vehicle.VehicleStatus = this.VehicleStatus;
            vehicle.GroupId = this.GroupId;
            vehicle.TownId = this.TownId;
            vehicle.DriverId = this.DriverId;
            vehicle.GpsEnabled = this.GpsEnabled;
            vehicle.Agent = this.Agent;
            if (PhotoFront != null)
            {
                vehicle.PhotoFront = await this.PhotoFront.GetPictureByteArray();
            }
            if (PhotoRear != null)
            {
                vehicle.PhotoRear = await PhotoRear.GetPictureByteArray();
            }
            if (PhotoAudit != null)
            {
                vehicle.PhotoAudit = await PhotoAudit.GetPictureByteArray();
            }
            if (PhotoInsuarance != null)
            {
                vehicle.PhotoInsuarance = await PhotoInsuarance.GetPictureByteArray();
            }
            if (PhotoGps != null)
            {
                vehicle.PhotoGps = await PhotoGps.GetPictureByteArray();
            }
            if (PhotoLicense != null)
            {
                vehicle.PhotoLicense = await PhotoLicense.GetPictureByteArray();

            }
            if (ExtraPhoto1 != null)
            {
                vehicle.ExtraPhoto1 = await ExtraPhoto1.GetPictureByteArray();
            }
            if (ExtraPhoto2 != null)
            {
                vehicle.ExtraPhoto2 = await ExtraPhoto2.GetPictureByteArray();
            }
            if (ExtraPhoto3 != null)
            {
                vehicle.ExtraPhoto3 = await ExtraPhoto3.GetPictureByteArray();
            }
        }
    }
}
