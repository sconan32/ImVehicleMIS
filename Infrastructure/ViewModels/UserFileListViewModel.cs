using Socona.ImVehicle.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Socona.ImVehicle.Web.ViewModels
{
    public class UserFileListViewModel
    {
       

        public UserFileListViewModel(UserFileItem t)
        {
            Id = t.Id;
            FileName = t.FileName;
            ServerPath = t.ServerPath;
            
            Name = t.Name;
            Size = t.Size;
            UploadDate = t.CreationDate;
            DownloadCount = t.DownloadCount;
            Visibility = t.Visibility;
        }

        public long Id { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "文件名")]
        public string FileName { get; set; }

        [Display(Name = "安全单位")]
        public string GroupName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "上传时间")]
        public DateTime? UploadDate { get; set; }


        [Display(Name = "服务器路径")]
        public string ServerPath { get; set; }


        [Display(Name = "下载次数")]
        public int DownloadCount { get; set; }

        [Display(Name = "文件大小")]
        public string SizeString
        {
            get
            {
                string[] units = { "GB", "MB", "KB", "B" };
                double sized = (double)Size;
                long divider = 1024 * 1024 * 1024;
                int uidx = 0;
                while (sized / divider < 1 && uidx < 3)
                {
                    uidx++;
                    divider /= 1024;
                }
                return (sized / divider).ToString("f3") + units[uidx];
            }
        }


        [Display(Name = "具体大小")]
        public long Size { get; set; }

        [Display(Name = "来源")]
        public VisibilityType Visibility { get; set; }
    }
}
