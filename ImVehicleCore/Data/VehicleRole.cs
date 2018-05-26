
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace Socona.ImVehicle.Core.Data
{
    public class VehicleRole : IdentityRole<string>
    {
        public bool Visible { get; set; }

        [MaxLength(256)]
        public string LocalName { get; set; }

        [MaxLength(256)]
        public string BaseRoleId { get; set; }

        [ForeignKey("BaseRoleId")]
        public virtual VehicleRole BaseRole { get; set; }
        //  public int? TownId { get; set; }

        //   [ForeignKey("TownId")]
        //   public virtual TownItem Town { get; set; }
        [Display(Name = "创建日期")]
        public DateTime? CreationDate { get; set; }
        [Display(Name = "修改日期")]
        public DateTime? ModificationDate { get; set; }

        [Display(Name = "修改用户")]
        [MaxLength(256)]
        public string CreateBy { get; set; }

        [Display(Name = "修改用户")]
        [MaxLength(256)]
        public string ModifyBy { get; set; }

        [Display(Name = "状态码")]
        public StatusType Status { get; set; }

    }
}