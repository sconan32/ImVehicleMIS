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


        [Display(Name = "身份证号")]
        public string IdCardNum { get; set; }

        [Display(Name = "职务")]
        public string Title { get; set; }

        [Display(Name = "单位")]
        public string Company { get; set; }

        [Display(Name = "住址")]
        public string Address { get; set; }

        [Display(Name = "户籍地")]
        public string RegisterAddress { get; set; }


        [Display(Name = "安全单位")]
        public long? GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual GroupItem Group { get; set; }

        [Display(Name = "街道")]
        public long? TownId { get; set; }

        [ForeignKey("TownId")]
        public virtual TownItem Town { get; set; }

    }
}
