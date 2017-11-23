using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImVehicleCore.Data
{
    public class SecurityPerson : BaseEntity
    {





        public string Tel { get; set; }

        public string Address { get; set; }
        

        public string RegisterAddress { get; set; }


        public long GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual GroupItem Group { get; set; }

    }
}
