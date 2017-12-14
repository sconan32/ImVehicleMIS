using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Web.ViewModels
{
    public class UserFileEditViewModel
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "文件名")]
        public string FileName { get; set; }

        [Display(Name = "安全单位")]
        public long? GroupId { get; set; }

        [Display(Name = "服务器路径")]
        public string ServerPath { get; set; }

        [Display(Name = "MIME格式")]
        public string ContentType { get; set; }

        [Display(Name = "下载次数")]
        public int DownloadCount { get; set; }

        [Required]
        [Display(Name = "文件")]
        public IFormFile UploadFile { get; set; }

        [Display(Name = "街道")]
        public long? TownId { get; set; }

        [Display(Name = "可见性")]
        public VisibilityType Visibility { get; set; }

      
    }
}
