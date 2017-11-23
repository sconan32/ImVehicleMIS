using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImVehicleCore.Data
{
    public class NewsItem : BaseEntity
    {

        public NewsItem()
        {
            Order = 1;
            PublishDate = DateTime.Now;
            ExpireDate = DateTime.Now.AddDays(7);
           
        }

        public string Title { get; set; }

       
        public string Content { get; set; }
        public string Excerpt { get; set; }
        public bool HasDateRange { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public int Order { get; set; }

    }
}
