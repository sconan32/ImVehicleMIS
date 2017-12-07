using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Socona.ImVehicle.Web.ViewModels
{
    public class NewsEditViewModel
    {
        [Display(Name = "标题")]
        public string Title { get; set; }

        [Display(Name = "内容")]
        public string Content { get; set; }

        [Display(Name = "来源")]
        public string Source { get; set; }
        public bool HasDateRange { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "发布日期")]

        public DateTime PublishDate { get; set; }
        [Display(Name = "失效日期")]
        [DataType(DataType.Date)]
        public DateTime? ExpireDate { get; set; }

        [Display(Name = "顺序")]
        public int Order { get; set; }
    }
}
