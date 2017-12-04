using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class TownItemListViewModel
    {
        public long Id { get; set; }
        [Display(Name = "编号")]
        public int Code { get; set; }
        [Display(Name = "名称")]
        public string Name { get; set; }
        [Display(Name = "安全单位数量")]
        public int GroupCount { get; set; }

        public bool IsValid { get; set; }
        [Display(Name = "安全状态",ShortName ="状态")]
        public string StatusText { get; set; }
        public int VehicleCount { get; set; }
        [Display(Name = "驾驶员数量")]
        public int DriverCount { get; set; }

    }
}
