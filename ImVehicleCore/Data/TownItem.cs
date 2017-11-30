using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImVehicleCore.Data
{
    public class TownItem : BaseEntity
    {





        public string Address { get; set; }



        public long DirstrictId { get; set; }

        [ForeignKey("DirstrictId")]
        public DistrictItem District { get; set; }


        public virtual List<GroupItem> Groups { get; set; }

        public virtual List<VehicleUser> Users { get; set; }

        public virtual List<DriverItem> Drivers { get; set; }
        
        public int Code { get; set; }


        public void AddSecurityItem (string name)
        {
            if(!Groups.Any(s=>s.Name==name))
            {
                Groups.Add(new GroupItem()
                {

                    
                });
            }
        }

    }
}
