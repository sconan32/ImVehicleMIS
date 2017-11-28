using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImVehicleCore.Data
{
    public class GroupItem : BaseEntity
    {




        public string Code { get; set; }

        public string Address { get; set; }

        public string RegisterAddress { get; set; }

        public string License { get; set; }

        public string Type { get; set; }

        public virtual List<SecurityPerson> SecurityPersons { get; set; }

        public virtual List<VehicleItem> Vehicles { get; set; }


        public long? TownId { get; set; }

        [ForeignKey("TownId")]
        public virtual TownItem Town { get; set; }


        public byte[] PhotoWarranty { get; set; }

        public byte[] PhotoSecurity { get; set; }



        public string ChiefCaption { get; set; }
        public string ChiefName { get; set; }

        public string ChiefTel { get; set; }

        public string Comment { get; set; }

        public byte[] PhotoMain { get; set; }

       

    }
}
