using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Socona.ImVehicle.Core.Data
{
    public class NewsItem : BaseEntity
    {


        [Required]
        [Display(Name = "标题")]
        [MaxLength(1024)]
        public string Title { get; set; }
        [Required]
        [Display(Name = "内容")]
        public string Content { get; set; }
        public string Excerpt { get; set; }
        public bool? HasDateRange { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "发布日期")]
        public DateTime PublishDate { get; set; }

        [MaxLength(256)]
        [Display(Name = "来源")]
        public string Source { get; set; }

        [Display(Name = "类型")]
        public NewsAreaType? Area { get; set; }

        [Display(Name = "失效日期")]
        [DataType(DataType.Date)]
        public DateTime? ExpireDate { get; set; }

        [Display(Name = "顺序")]
        public int? Order { get; set; }

        [Display(Name = "图片")]
        public virtual UserFileItem ImageFile { get; set; }

        [ForeignKey(nameof(ImageFile))]
        public long? ImageFileId { get; set; }



    }

    public enum NewsAreaType
    {
        [Display(Name = "通知·通报")]
        Notification = 1,
        [Display(Name = "法律·法规")]
        LawAndRule = 2,
        [Display(Name = "交通安全常识")]
        SecurityKnowledge = 3,
        [Display(Name = "事故案例")]
        AccidentCase = 4,
        [Display(Name = "图片新闻")]
        ImageNews = 9,

    }
}
