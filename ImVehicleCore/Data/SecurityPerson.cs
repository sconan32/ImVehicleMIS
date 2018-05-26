using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Socona.ImVehicle.Core.Data
{
    public class SecurityPerson : BaseEntity
    {




        [Display(Name = "电话")]
        [MaxLength(64)]
        public string Tel { get; set; }


        [Display(Name = "身份证号")]
        [MaxLength(128)]
        public string IdCardNum { get; set; }

        [Display(Name = "职务")]
        [MaxLength(32)]
        public string Title { get; set; }

        [Display(Name = "单位")]
        [MaxLength(512)]
        public string Company { get; set; }

        [Display(Name = "住址")]
        [MaxLength(2048)]
        public string Address { get; set; }

        [Display(Name = "户籍地")]
        [MaxLength(2048)]
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
