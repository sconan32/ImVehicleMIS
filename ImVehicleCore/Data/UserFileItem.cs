using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ImVehicleCore.Data
{
    public class UserFileItem : BaseEntity
    {
        [Display(Name = "文件大小")]
        public long Size { get; set; }
        [Display(Name = "服务器路径")]
        public string ServerPath { get; set; }
        [Display(Name = "客户端路径")]
        public string ClientPath { get; set; }
        [Display(Name = "文件类型")]
        public string Type { get; set; }
        [Display(Name = "文件名")]
        public string FileName { get; set; }

        public int DownloadCount { get; set; }

        public long? GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual GroupItem Group { get; set; }

        public long? TownId;

        [ForeignKey("TownId")]
        public virtual TownItem Town { get; set; }

        [Display(Name = "MIME格式")]
        public string ContentType { get; set; }
    }
}
