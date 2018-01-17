using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Web.ViewModels
{
    public class GroupDetailViewModel
    {
        public GroupDetailViewModel(GroupItem group=null)
        {
            OriginalModel = group;
            if (group!=null)
            {

                Id = group.Id;
                Name = group.Name;
                Address = group.Address;
                RegisterAddress = group.RegisterAddress;
                License = group.License;
                ChiefName = group.ChiefName;
                ChiefTel = group.ChiefTel;
                Type = group.Type;
                Introduction = group.Comment;

                PhotoMain = group.PhotoMain != null ? Convert.ToBase64String(group.PhotoMain) : "";
                PhotoWarranty = group.PhotoWarranty != null ? Convert.ToBase64String(group.PhotoWarranty) : "";
                PhotoSecurity = group.PhotoSecurity != null ? Convert.ToBase64String(group.PhotoSecurity) : "";

                TownName = group.Town.Name;
                VehicleCount = group.Vehicles.Count;

                DriverCount = group.Drivers.Count;
                SecuremanCount = group.Drivers.Count;
                DriverInvalidCount = group.Drivers.Count(d => !d.IsValid());
                VehicleInvalidCount = group.Vehicles.Count(v => !v.IsValid());
                IsValid = group.IsValid();


                Vehicles = group.Vehicles.Select(t => new VehicleListViewModel(t)).ToList();
                Drivers = group.Drivers.Select(t => new DriverListViewModel(t)).ToList();
                UserFiles = group.UserFiles.Select(t => new UserFileListViewModel(t)).ToList();

                Securemans = group.SecurityPersons.Select(t => new SecureManListViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Address = t.Address,
                    RegisterAddress = t.RegisterAddress,
                    Company = t.Company,
                    GroupName = t.Group?.Name,
                    TownName = t.Town?.Name,
                    IdCardNum = t.IdCardNum,
                    Tel = t.Tel,
                    Title = t.Title,
                }).ToList();
            }
        }
        public long Id { get; set; }
        [Display(Name = "名称")]
        public string Name { get; set; }
        [Display(Name = "办公地址")]
        public string Address { get; set; }
        [Display(Name = "注册地址")]
        public string RegisterAddress { get; set; }
        [Display(Name = "注册号")]
        public string License { get; set; }

        [Display(Name = "负责人")]
        public string ChiefName { get; set; }
        [Display(Name = "负责人电话")]
        public string ChiefTel { get; set; }

        [Display(Name = "单位类型")]
        public string Type { get; set; }

        [Display(Name = "企业图像")]
        public string PhotoMain { get; set; }
        [Display(Name = "资质凭证")]
        public string PhotoWarranty { get; set; }
        [Display(Name = "安全生产凭证")]
        public string PhotoSecurity { get; set; }

        [Display(Name = "注册车辆数目")]
        public int VehicleCount { get; set; }

        [Display(Name = "所属街道")]
        public string TownName { get; set; }

        [Display(Name = "驾驶员数目")]
        public int DriverCount { get; set; }


        [Display(Name = "安全员数目")]
        public int SecuremanCount { get; set; }

        [Display(Name = "公司介绍")]
        public string Introduction { get; set; }

        [Display(Name = "预警车辆数")]
        public int VehicleInvalidCount { get; set; }

        [Display(Name = "预警司机数")]
        public int DriverInvalidCount { get; set; }


        public List<VehicleListViewModel> Vehicles { get; set; }

        public List<DriverListViewModel> Drivers { get; set; }


        public List<UserFileListViewModel> UserFiles { get; set; }

        public List<SecureManListViewModel> Securemans { get; set; }


        public bool IsValid { get; set; }

        public GroupItem OriginalModel { get; set; }
    }
}
