using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Socona.ImVehicle.Core.Data
{
    public class GroupItem : BaseEntity
    {




        [Display(Name = "编码")]
        [MaxLength(64)]
        public string Code { get; set; }


        [Display(Name = "办公地址")]
        [MaxLength(2048)]
        public string Address { get; set; }
        [Display(Name = "注册地址")]
        [MaxLength(2048)]
        public string RegisterAddress { get; set; }
        [Display(Name = "纳税人识别号")]
        [MaxLength(256)]
        public string License { get; set; }
        [Display(Name = "单位类型")]
        [MaxLength(128)]
        public string Type { get; set; }


        public virtual List<SecurityPerson> SecurityPersons { get; set; }

        public virtual List<VehicleItem> Vehicles { get; set; }

        public virtual List<DriverItem> Drivers { get; set; }

  
        public virtual List<UserFileItem> UserFiles { get; set; }

        public long? TownId { get; set; }

        [ForeignKey("TownId")]
        public virtual TownItem Town { get; set; }

      

    
        [Display(Name = "文件附件")]
        [MaxLength(2048)]
        public string AttachmentFilePath { get; set; }
        [Display(Name = "职务")]
        [MaxLength(64)]
        public string ChiefTitle { get; set; }
        [Display(Name = "负责人")]
        [MaxLength(32)]
        public string ChiefName { get; set; }
        [MaxLength(64)]
        [Display(Name = "电话")]
        public string ChiefTel { get; set; }
        
        [Display(Name = "介绍")]
        public string Comment { get; set; }

        [Display(Name = "监理民警")]
        [MaxLength(32)]
        public string Policeman { get; set; }
        [Display(Name = "监理中队")]
        [MaxLength(128)]
        public string PoliceOffice { get; set; }


        public long? MainImageId { get; set; }

        [Display(Name = "企业图片")]
        [ForeignKey(nameof(MainImageId))]
        public virtual UserFileItem MainImage { get; set; }


        public long? LicenseImageId { get; set; }

        [Display(Name = "证照扫描件")]
        [ForeignKey(nameof(LicenseImageId))]
        public virtual UserFileItem LicenseImage { get; set; }

        
        public long? ExtraImage1Id { get; set; }

        [Display(Name = "附加图片1")]
        [ForeignKey(nameof(ExtraImage1Id))]
        public virtual UserFileItem ExtraImage1 { get; set; }


        public long? ExtraImage2Id { get; set; }

        [Display(Name = "附加图片2")]
        [ForeignKey(nameof(ExtraImage2Id))]
        public virtual UserFileItem ExtraImage2 { get; set; }

        public long? ExtraImage3Id { get; set; }

        [Display(Name = "附加图片3")]
        [ForeignKey(nameof(ExtraImage3Id))]
        public virtual UserFileItem ExtraImage3 { get; set; }
      
        [Display(Name = "资质审核文件")]
        public long? ApplicationFileId { get; set; }
        [ForeignKey("ApplicationFileId")]
        public virtual UserFileItem ApplicationFile { get; set; }

        [Display(Name = "规章制度文件")]
        public long? RuleFileId { get; set; }

        [ForeignKey("RuleFileId")]
        public virtual UserFileItem RuleFile { get; set; }


        [Display(Name = "驾驶员责任状")]
        public long? DriverGuranteeFileId { get; set; }
        [ForeignKey("DriverGuranteeFileId")]
        public virtual UserFileItem DriverGuranteeFile { get; set; }


        [Display(Name = "企业责任状")]
        public long? GroupGuranteeFileId { get; set; }
        [ForeignKey("GroupGuranteeFileId")]
        public virtual UserFileItem GroupGuranteeFile { get; set; }

        public bool IsValid()
        {

            if (Vehicles != null)
            {
                if (Vehicles.Count(v => !v.IsValid()) > 0)
                {
                    return false;
                }
            }
            if (Drivers != null)
            {
                if (Drivers.Count(d => !d.IsValid()) > 0)
                {
                    return false;
                }
            }
            return true;
        }
        public virtual  List<VehicleUser> Users { get; set; }
    }
}
