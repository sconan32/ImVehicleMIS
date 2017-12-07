using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Socona.ImVehicle.Core.Data
{
    public class NewsItem : BaseEntity
    {

       
        [Required]
        [Display(Name = "标题")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "内容")]
        public string Content { get; set; }
        public string Excerpt { get; set; }
        public bool? HasDateRange { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "发布日期")]
        public DateTime PublishDate { get; set; }

        [Display(Name = "来源")]
        public string Source { get; set; }


        [Display(Name = "失效日期")]
        [DataType(DataType.Date)]
        public DateTime? ExpireDate { get; set; }

        [Display(Name = "顺序")]
        public int Order { get; set; }

    }
}
