using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ImVehicleCore.Data;

namespace Web.ViewModels
{
    public class DriverListViewModel
    {
        public DriverListViewModel(DriverItem t=null)
        {
            var nowDate = DateTime.Now.Date;
            if (t != null)
            {
                Id = t.Id;
                Name = t.Name;
                IdCardNumber = t.IdCardNumber;
                License = t.LicenseNumber;
                LicenseType = t.LicenseType;
                LicenseIssue = t.LicenseIssueDate;
                ValidYears = t.LicenseValidYears;
                Gender = t.Gender;
                VehiclesRegistered = t.Vehicles?.Count ?? 0;
                Tel = t.Tel;
                IsValid = (t.LicenseIssueDate.AddYears(t.LicenseValidYears) >= nowDate);
            }
        }

        public long Id { get; set; }

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

        [Display(Name = "住址")]
        public string LivingAddress { get; set; }
        [Display(Name = "职务")]
        public string Title { get; set; }

        [Display(Name = "首次申领驾驶证日期")]
        public DateTime FirstLicenseIssueDate { get; set; }

        [Display(Name = "驾驶证签发日期")]
        public DateTime LicenseIssueDate { get; set; }
        [Display(Name = "驾驶证有效年限")]
        public int LicenseValidYears { get; set; }

        [Display(Name = "街道")]
        public string TownName { get; set; }

        [Display(Name = "安全单位")]
        public string GroupName { get; set; }

        public bool IsValid { get; private set; }
    }

}
