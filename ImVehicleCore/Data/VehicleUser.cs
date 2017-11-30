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

        public long? TownId { get; set; }

        [ForeignKey("TownId")]
        public virtual TownItem Town { get; set; }
    }
}
