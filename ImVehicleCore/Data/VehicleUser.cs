using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ImVehicleCore.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class VehicleUser : IdentityUser<string>
    {

        public string RealName { get; set; }

        public string Serial { get; set; }

        public string Depart { get; set; }


       

        public long? TownId { get; set; }

        [ForeignKey("TownId")]
        public virtual TownItem Town { get; set; }
    }
}
