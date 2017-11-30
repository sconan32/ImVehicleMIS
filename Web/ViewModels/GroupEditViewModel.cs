using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Web.ViewModels
{
    public class GroupEditViewModel
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "编号")]
        public string Code { get; set; }

        [Required]
        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "办公地址")]
        public string Address { get; set; }

        [Display(Name = "注册地址")]
        public string RegisterAddress { get; set; }

        [Display(Name = "注册号")]
        public string License { get; set; }

        [Required]
        [Display(Name = "负责人")]
        public string ChiefName { get; set; }

        [Display(Name = "负责人电话")]
        public string ChiefTel { get; set; }

        [Required]
        [Display(Name = "单位类型")]
        public string Type { get; set; }

        [Display(Name = "企业图像")]
        public IFormFile PhotoMain { get; set; }

        public string PhotoMainBase64 { get; set; }



        [Display(Name = "资质凭证")]
        public IFormFile PhotoWarranty { get; set; }

        public string PhotoWarrantyBase64 { get; set; }

        [Display(Name = "安全生产凭证")]
        public IFormFile PhotoSecurity { get; set; }
        public string PhotoSecurityBase64 { get; set; }


        [Display(Name = "所属街道")]
        public long TownId { get; set; }

        [Display(Name = "负责人职务")]
        public string ChiefTitle { get; set; }

        [Display(Name = "企业介绍")]
        public string Comment { get; set; }
    }
}
