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


        public bool IsValid()
        {
            if(Groups?.Count(g=>!g.IsValid())>0)
            {
                return false;
            }
            if(Drivers?.Count(d=>!d.IsValid())>0)
            {
                return false;
            }

            return true;
        }

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
