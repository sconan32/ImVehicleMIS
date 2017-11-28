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




        [Display(Name = "电话")]
        public string Tel { get; set; }

        [Display(Name = "住址")]
        public string Address { get; set; }

        [Display(Name = "户籍地")]
        public string RegisterAddress { get; set; }


        public long GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual GroupItem Group { get; set; }

    }
}
