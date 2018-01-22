using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;

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

        public string Type { get; set; }

        [ForeignKey("DistrictId")]
        public virtual DistrictItem District { get; set; }

        public long? DistrictId { get; set; }

        [Display(Name = "创建日期")]
        public DateTime? CreationDate { get; set; }
        [Display(Name = "修改日期")]
        public DateTime? ModificationDate { get; set; }

        [Display(Name = "修改用户")]
        public string CreateBy { get; set; }

        [Display(Name = "修改用户")]
        public string ModifyBy { get; set; }

        [Display(Name = "状态码")]
        public StatusType Status { get; set; }


        public async Task<bool> CanDoAsync(string operation, BaseEntity entity, UserManager<VehicleUser> userManager)
        {
            if (entity is TownItem)
            {
                return await CanDoAsync(operation, entity as TownItem, userManager);
            }
            else if (entity is GroupItem)
            {
                return await CanDoAsync(operation, entity as GroupItem, userManager);
            }
            else if (entity is DriverItem)
            {
                return await CanDoAsync(operation, entity as DriverItem, userManager);
            }
            else if (entity is VehicleItem)
            {
                return await CanDoAsync(operation, entity as VehicleItem, userManager);
            }
            return false;
        }

        public async Task<bool> CanDoAsync(string operation, TownItem town, UserManager<VehicleUser> userManager)
        {
            if (operation == Constants.ReadOperationName)
            {
                return await CanDo(town, userManager, t => true, t => true, t => t.Id == TownId, t => false);

            }
            else if (operation == Constants.CreateOperationName || operation == Constants.UpdateOperationName || operation == Constants.DeleteOperationName)
            {
                return await CanDo(town, userManager, t => true, t => false, t => t.Id == TownId, t => false);
            }
            return false;

        }
        public async Task<bool> CanDoAsync(string operation, GroupItem group, UserManager<VehicleUser> userManager)
        {
            if (operation == Constants.ReadOperationName)
            {
                return await CanDo(group, userManager, t => true, t => true, t => t.TownId == TownId, t => t.Id == GroupId);

            }
            else if (operation == Constants.CreateOperationName || operation == Constants.UpdateOperationName || operation == Constants.DeleteOperationName)
            {
                return await CanDo(group, userManager, t => true, t => false, t => t.Id == TownId, t => false);
            }
            else if (operation == Constants.UploadUserFileOperationName)
            {
                return await CanDo(group, userManager, t => true, t => false, t => t.Id == TownId, t => t.Id == GroupId);
            }
            return false;

        }

        public async Task<bool> CanDoAsync(string operation, DriverItem driver, UserManager<VehicleUser> userManager)
        {
            if (operation == Constants.ReadOperationName)
            {
                return await CanDo(driver, userManager, t => true, t => true, t => t.TownId == TownId, t => t.GroupId == GroupId);

            }
            else if (operation == Constants.CreateOperationName || operation == Constants.UpdateOperationName || operation == Constants.DeleteOperationName)
            {
                return await CanDo(driver, userManager, t => true, t => false, t => t.Id == TownId, t => false);
            }
            return false;
        }
        public async Task<bool> CanDoAsync(string operation, VehicleItem vehicle, UserManager<VehicleUser> userManager)
        {
            if (operation == Constants.ReadOperationName)
            {
                return await CanDo(vehicle, userManager, t => true, t => true, t => t.TownId == TownId, t => t.GroupId == GroupId);

            }
            else if (operation == Constants.CreateOperationName || operation == Constants.UpdateOperationName || operation == Constants.DeleteOperationName)
            {
                return await CanDo(vehicle, userManager, t => true, t => false, t => t.Id == TownId, t => false);
            }
            return false;
        }


        private async Task<bool> CanDo<T>(T item, UserManager<VehicleUser> userManager,
            Func<T, bool> adminAction,
             Func<T, bool> globalVisitorAction,
             Func<T, bool> townManagerAction,
             Func<T, bool> groupManagerAction)
        {
            if (await userManager.IsInRoleAsync(this, "Admins"))
            {
                return adminAction.Invoke(item);
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
