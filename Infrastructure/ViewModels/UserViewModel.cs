using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Infrastructure.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel() { }
        public UserViewModel(VehicleUser user)
        {
            Id = user.Id;
            Name = user.UserName;
            Email = user.Email;
            Type = user.Type;
            GroupId = user.GroupId;
            TownId = user.TownId;
            PhoneNumber = user.PhoneNumber;
            StatusCode = user.Status;
            switch (user.Status)
            {
                case StatusType.OK:
                    Status = "正常";
                    break;
                case StatusType.Forbidden:
                    Status = "停用";
                    break;
                case StatusType.Authorizing:
                    Status = "等待审核";
                    break;
            }
        }
  
        public string Id { get; private set; }
        [Display(Name = "用户名")]
        public string Name { get; set; }
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Display(Name = "类型")]
        public string Type { get; set; }
       

        public GroupItem Group { get; set; }
        [Display(Name = "状态")]

        public string Status { get; set; }
        [Display(Name = "所属安全组")]
        public string GroupName { get; set; }

        public long? GroupId { get; set; }
        [Display(Name = "所属街道")]
        public string TownName { get; set; }

        public long? TownId { get; set; }


        public long? TownItem { get; set; }
        [Display(Name = "电话")]
        public string PhoneNumber { get; set; }

        public StatusType StatusCode { get; set; }
    }
}
