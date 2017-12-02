using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ImVehicleCore.Data
{
    // This can easily be modified to be BaseEntity<T> and public T Id to support different key types.
    // Using non-generic integer types for simplicity and to ease caching logic
    public class BaseEntity
    {

        [Display(Name = "#")]
        [Key]
        public long Id { get; set; }
        [Display(Name = "版本号")]
        public int VersionNumber { get; set; }
        [Display(Name = "元数据")]
        public string Metadata { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "创建日期")]
        public DateTime CreationDate { get; set; }
        [Display(Name = "修改日期")]
        public DateTime? ModificationDate { get; set; }

        [Display(Name = "修改用户")]
        public string CreateBy { get; set; }

        [Display(Name = "修改用户")]
        public string ModifyBy { get; set; }

        [Display(Name = "状态码")]
        public StatusType Status { get; set; }
    }


    public enum StatusType
    {
        [Display(Name = "正常")]
        OK =200,
        [Display(Name = "删除")]
        Deleted =404,

        [Display(Name = "待审")]
        Authorizing=401,
        [Display(Name = "脏")]
        Dirty =302,
        [Display(Name = "出错")]
        Error =500,

    }
}
