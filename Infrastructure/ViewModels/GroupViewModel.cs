using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Infrastructure.Extensions;

namespace Socona.ImVehicle.Web.ViewModels
{
    public class GroupViewModel
    {
        public GroupViewModel() { }
        public GroupViewModel(GroupItem group)
        {

            OriginalModel = group;

            Id = group.Id;
            Code = group.Code;
            Name = group.Name;
            Address = group.Address;
            RegisterAddress = group.RegisterAddress;
            License = group.License;
            ChiefName = group.ChiefName;
            ChiefTitle = group.ChiefTitle;
            ChiefTel = group.ChiefTel;
            TownId = group.TownId;
            TownName = group.Town?.Name;
            Type = group.Type;
            Comment = group.Comment;
            Policeman = group.Policeman;
            PoliceOffice = group.PoliceOffice;
            ApplicationFileId = group.ApplicationFileId;
            RuleFileId = group.RuleFileId;
            DriverGuranteeFileId = group.DriverGuranteeFileId;
            GroupGuranteeFileId = group.GroupGuranteeFileId;



            PhotoMainBase64 = group.PhotoMain.ToBase64String();
            PhotoSecurityBase64 = group.PhotoSecurity.ToBase64String();
            PhotoWarrantyBase64 = group.PhotoWarranty.ToBase64String();
            ExtraPhoto1Base64 = group.ExtraPhoto1.ToBase64String();
            ExtraPhoto2Base64 = group.ExtraPhoto2.ToBase64String();
            ExtraPhoto3Base64 = group.ExtraPhoto3.ToBase64String();

            VehicleCount = group.Vehicles?.Count ?? 0;
            DriverCount = group.Drivers?.Count ?? 0;
            SecuremanCount = group.Drivers?.Count ?? 0;
            DriverInvalidCount = group.Drivers?.Count(d => !d.IsValid()) ?? 0;
            VehicleInvalidCount = group.Vehicles?.Count(v => !v.IsValid()) ?? 0;
            IsValid = group.IsValid();
        }

        public GroupItem OriginalModel { get; set; }

        public long Id { get; set; }


        [Display(Name = "编号")]
        public string Code { get; set; }

        [Required]
        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "办公地址")]
        public string Address { get; set; }

        [Display(Name = "注册地址")]
        public string RegisterAddress { get; set; }

        [Display(Name = "法人识别号")]
        public string License { get; set; }

        [Display(Name = "负责人")]
        public string ChiefName { get; set; }

        [Display(Name = "负责人电话")]
        public string ChiefTel { get; set; }


        [Display(Name = "单位类型")]
        public string Type { get; set; }

        [Display(Name = "企业图像")]
        public IFormFile PhotoMain { get; set; }

        public string PhotoMainBase64 { get; private set; }


        [Display(Name = "资质凭证")]
        public IFormFile PhotoWarranty { get; set; }

        public string PhotoWarrantyBase64 { get; private set; }

        [Display(Name = "安全生产凭证")]
        public IFormFile PhotoSecurity { get; set; }
        public string PhotoSecurityBase64 { get; private set; }


        [Display(Name = "所属街道")]
        public long? TownId { get; set; }


        [Display(Name = "从属街道")]
        public string TownName { get; private set; }


        [Display(Name = "负责人职务")]
        public string ChiefTitle { get; set; }

        [Display(Name = "企业介绍")]
        public string Comment { get; set; }

        public bool IsValid { get; set; }


        [Display(Name = "监理民警")]
        public string Policeman { get; set; }
        [Display(Name = "监理中队")]
        public string PoliceOffice { get; set; }


        [Display(Name = "安全员数目")]
        public int SecuremanCount { get; set; }


        [Display(Name = "预警车辆数")]
        public int VehicleInvalidCount { get; set; }

        [Display(Name = "预警司机数")]
        public int DriverInvalidCount { get; set; }



        public long? ApplicationFileId { get; set; }

        [Display(Name = "资质审核文件")]
        public virtual IFormFile ApplicationFile { get; set; }


        public long? RuleFileId { get; set; }
        [Display(Name = "规章制度")]
        public virtual IFormFile RuleFile { get; set; }



        public long? DriverGuranteeFileId { get; set; }
        [Display(Name = "驾驶员保证书")]
        public virtual IFormFile DriverGuranteeFile { get; set; }



        public long? GroupGuranteeFileId { get; set; }
        [Display(Name = "企业保证书")]
        public virtual IFormFile GroupGuranteeFile { get; set; }

        [Display(Name = "附加照片1")]
        public IFormFile ExtraPhoto1 { get; set; }
        public string ExtraPhoto1Base64 { get; private set; }

        [Display(Name = "附加照片2")]
        public IFormFile ExtraPhoto2 { get; set; }
        public string ExtraPhoto2Base64 { get; private set; }

        [Display(Name = "附加照片3")]
        public IFormFile ExtraPhoto3 { get; set; }
        public string ExtraPhoto3Base64 { get; private set; }


        [Display(Name = "注册车辆数")]
        public int VehicleCount { get; private set; }

        [Display(Name = "预警车辆数")]
        public int InvalidVehicleCount { get; private set; }
        [Display(Name = "注册驾驶员数")]
        public int DriverCount { get; private set; }
        [Display(Name = "预警驾驶员数")]
        public int InvalidDriverCount { get; private set; }


        public async Task FillGroupItem(GroupItem group)
        {
            group.Code = this.Code;
            group.Name = this.Name;
            group.Address = this.Address;
            group.RegisterAddress = this.RegisterAddress;
            group.License = this.License;
            group.ChiefName = this.ChiefName;
            group.ChiefTel = this.ChiefTel;
            group.ChiefTitle = this.ChiefTitle;
            group.Type = this.Type;
            group.TownId = this.TownId;
            group.Comment = this.Comment;
            group.Policeman = this.Policeman;
            group.PoliceOffice = this.PoliceOffice;

            if (PhotoMain != null)
            {
                group.PhotoMain = await PhotoMain.GetPictureByteArray();
            }
            if (PhotoSecurity != null)
            {
                group.PhotoSecurity = await PhotoSecurity.GetPictureByteArray();
            }
            if (PhotoWarranty != null)
            {
                group.PhotoWarranty = await PhotoWarranty.GetPictureByteArray();
            }
            if (ExtraPhoto1 != null)
            {
                group.ExtraPhoto1 = await ExtraPhoto1.GetPictureByteArray();
            }
            if (ExtraPhoto2 != null)
            {
                group.ExtraPhoto2 = await ExtraPhoto2.GetPictureByteArray();
            }
            if (ExtraPhoto3 != null)
            {
                group.ExtraPhoto3 = await ExtraPhoto3.GetPictureByteArray();
            }

        }
    }
}
