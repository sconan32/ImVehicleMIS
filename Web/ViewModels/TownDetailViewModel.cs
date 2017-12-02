using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ImVehicleCore.Data;

namespace Web.ViewModels
{
    public class TownDetailViewModel
    {

        public TownDetailViewModel(TownItem t)
        {

        }
        public long Id { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }
        [Display(Name = "安全单位数量")]
        public int GroupCount { get; set; }

        [Display(Name = "车辆数量")]
        public int VehicleCount { get; set; }
        [Display(Name = "驾驶员数量")]
        public int DriverCount { get; set; }

        public int ValidCount { get; set; }

        public int InvalidCount { get; set; }

        public int ValidCount2 { get; set; }
        public int InvalidCount2 { get; set; }

        public int ValidCount3 { get; set; }
        public int InvalidCount3 { get; set; }
        public List<GroupListViewModel> Groups { get; set; }

        public List<DriverListViewModel> Drivers { get; set; }


        public List<VehicleListViewModel> Vehicles { get; set; }
    }
}
