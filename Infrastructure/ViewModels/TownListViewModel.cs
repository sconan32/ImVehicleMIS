﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Socona.ImVehicle.Web.ViewModels
{
    public class TownListViewModel
    {
        public long Id { get; set; }
        [Display(Name = "编号")]
        public int Code { get; set; }
        [Display(Name = "名称")]
        public string Name { get; set; }
        [Display(Name = "安全单位数量")]
        public int GroupCount { get; set; }

        public bool IsValid { get; set; }
        [Display(Name = "状态")]
        public string StatusText { get; set; }
        [Display(Name = "注册车辆数量")]
        public int VehicleCount { get; set; }
        [Display(Name = "驾驶员数量")]
        public int DriverCount { get; set; }

    }
}
