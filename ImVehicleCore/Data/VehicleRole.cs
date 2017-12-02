
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace ImVehicleCore.Data
{
    public class VehicleRole : IdentityRole<string>
    {
        public bool Visible { get; set; }

        public string LocalName { get; set; }


        public string BaseRoleId { get; set; }

        [ForeignKey("BaseRoleId")]
        public virtual VehicleRole BaseRole { get; set; }
        //  public int? TownId { get; set; }

        //   [ForeignKey("TownId")]
        //   public virtual TownItem Town { get; set; }

    }
}