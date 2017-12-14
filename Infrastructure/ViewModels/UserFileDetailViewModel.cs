using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Socona.ImVehicle.Web.ViewModels
{
    public class UserFileDetailViewModel
    {
        public long Id { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "文件名")]
        public string FileName { get; set; }

        [Display(Name = "安全单位")]
        public string GroupName { get; set; }


        [Display(Name = "服务器路径")]
        public string ServerPath { get; set; }


        [Display(Name = "下载次数")]
        public int DownloadCount { get; set; }

        [Display(Name = "MIME格式")]
        public string ContentType { get; set; }

        [Display(Name = "文件")]
        IFormFile UploadFile { get; set; }


      
    }
}
