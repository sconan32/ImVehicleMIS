using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImVehicleCore.Data
{
    public class GroupItem : BaseEntity
    {




        [Display(Name = "编码")]    
        public string Code { get; set; }


        [Display(Name = "办公地址")]    
        public string Address { get; set; }
        [Display(Name = "注册地址")]
        public string RegisterAddress { get; set; }
        [Display(Name = "纳税人识别号")]
        public string License { get; set; }
        [Display(Name = "单位类型")]
        public string Type { get; set; }

        public virtual List<SecurityPerson> SecurityPersons { get; set; }

        public virtual List<VehicleItem> Vehicles { get; set; }

        public virtual List<DriverItem> Drivers { get; set; }

        public virtual List<UserFile> UserFiles { get; set; }

        public long? TownId { get; set; }

        [ForeignKey("TownId")]
        public virtual TownItem Town { get; set; }

        [Display(Name = "资质照片")]
        public byte[] PhotoWarranty { get; set; }

        [Display(Name = "安全资质照片")]
        public byte[] PhotoSecurity { get; set; }
        [Display(Name = "文件附件")]
        public string AttachmentFilePath { get; set; }
        [Display(Name = "职务")]
        public string ChiefTitle { get; set; }
        [Display(Name = "负责人")]
        public string ChiefName { get; set; }
        [Display(Name = "电话")]
        public string ChiefTel { get; set; }
        [Display(Name = "介绍")]
        public string Comment { get; set; }
        [Display(Name = "企业照片")]
        public byte[] PhotoMain { get; set; }

      

    }
}
