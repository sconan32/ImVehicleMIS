using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Data;

namespace Web.ViewModels
{
    public class TownDetailViewModel
    {

        public TownDetailViewModel(TownItem town = null)
        {
            if (town == null)
            {
                return;
            }

            Id = town.Id;
            Name = town.Name;
            GroupCount = town.Groups.Count;
            Groups = town.Groups.Select(t => new GroupListViewModel(t)).ToList();
            Drivers = town.Drivers.Select(t => new DriverListViewModel(t)).ToList();
            Vehicles = town.Groups.SelectMany(g => g.Vehicles).Select(t => new VehicleListViewModel(t)).ToList();

            DriverCount = Drivers.Count;
            VehicleCount = Vehicles.Count;
            InvalidGroupCount = Groups.Count(t => !t.IsValid);
            InvalidDriverCount = Drivers.Count(t => !t.IsValid);
            InvalidVehicleCount = Vehicles.Count(t => !t.IsValid);
            IsValid = (InvalidGroupCount <= 0) && (InvalidDriverCount <= 0) && (InvalidVehicleCount <= 0);
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


        [Display(Name = "预警数量")]
        public int InvalidGroupCount { get; set; }
        [Display(Name = "预警数量")]

        public int InvalidDriverCount { get; set; }

        [Display(Name = "预警数量")]
        public int InvalidVehicleCount { get; set; }


        public bool IsValid { get; set; }

        public List<GroupListViewModel> Groups { get; set; }

        public List<DriverListViewModel> Drivers { get; set; }


        public List<VehicleListViewModel> Vehicles { get; set; }
    }
}
