﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ImVehicleCore.Data;

namespace Web.ViewModels
{
    public class DriverListViewModel
    {
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



    }

}
