using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class GroupListViewModel
    {
        public long Id { get; set; }

        [Display(Name ="从属街道")]
        public string TownName { get; set; }

        [Display(Name="代码")]
        public string Code { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }
        
        [Display(Name = "负责人")]
        public string ChiefName { get; set; }

        [Display(Name = "负责人头衔")]
        public string ChiefTitle { get; set; }

        [Display(Name = "负责人电话")]
        public string ChiefTel { get; set; }

        [Display(Name = "注册车辆数")]
        public int VehicleCount { get; set; }

        [Display(Name = "预警车辆数")]
        public int InvalidCount { get; set; }

        [Display(Name = "单位类型")]
        public string Type { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }

        [Display(Name = "纳税人识别号")]
        public string License { get; set; }

     
        public bool IsValid { get; set; }

    }
}
