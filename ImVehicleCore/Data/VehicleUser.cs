using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace Socona.ImVehicle.Core.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class VehicleUser : IdentityUser<string>
    {

        public string RealName { get; set; }

        public string Serial { get; set; }

        public string Depart { get; set; }

        public string Company { get; set; }


        public string Title { get; set; }


        public long? TownId { get; set; }

        [ForeignKey("TownId")]
        public virtual TownItem Town { get; set; }

        [ForeignKey("GroupId")]
        public virtual GroupItem Group { get; set; }

        public long? GroupId { get; set; }

        [ForeignKey("DistrictId")]
        public virtual DistrictItem District {get;set;}

        public long? DistrictId { get; set; }


        public async Task<bool> CanViewAsync(TownItem town,UserManager<VehicleUser> userManager)
        {
            return await CanView<TownItem>(town,userManager, t => true, t => true, t => t.Id == TownId, t => false);
        }
        public async Task<bool> CanViewAsync(GroupItem group, UserManager<VehicleUser> userManager)
        {
            return await CanView<GroupItem>(group, userManager, t => true, t => true, t => t.TownId==TownId, t => t.Id==GroupId);
        }

        public async Task<bool> CanViewAsync(DriverItem driver,UserManager<VehicleUser> userManager)
        {
            return await CanView<DriverItem>(driver, userManager, t => true, t => true, t => t.TownId == TownId, t => t.GroupId == GroupId);
        }
        public async Task<bool> CanViewAsync(VehicleItem vehicle, UserManager<VehicleUser> userManager)
        {
            return await CanView<VehicleItem>(vehicle, userManager, t => true, t => true, t => t.TownId == TownId, t => t.GroupId == GroupId);
        }
        private async Task< bool> CanView<T>(T item, UserManager<VehicleUser> userManager,
            Func<T,bool> adminAction,
             Func<T,bool> globalVisitorAction,
             Func<T,bool> townManagerAction,
             Func<T,bool> groupManagerAction     )
        {
            if(await userManager.IsInRoleAsync(this, "Admins"))
            {
                return adminAction.Invoke(item) ;
            }
            if (await userManager.IsInRoleAsync(this, "GlobalVisitor"))
            {
                return globalVisitorAction.Invoke(item);
            }
            if (await userManager.IsInRoleAsync(this, "TownManager"))
            {
                return townManagerAction.Invoke(item);
            }
            if (await userManager.IsInRoleAsync(this, "GroupManager"))
            {
                return groupManagerAction.Invoke(item);
            }
            return false;
        }
    }
}
