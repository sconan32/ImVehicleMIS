using Microsoft.AspNetCore.Http;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Socona.ImVehicle.Web.ViewModels
{
    public class NewsEditViewModel
    {
        public NewsEditViewModel() { }

        public NewsEditViewModel(NewsItem news)
        {
            this.Id = news.Id;
            this.Content = news.Content;
            this.Title = news.Title;
            this.Source = news.Source;
            this.Area = news.Area;
            this.PublishDate = news.PublishDate;
            this.ExpireDate = news.ExpireDate;
            this.Order = news.Order;
            this.ImageBase64 = news.ImageFile.ToBase64String();


        }


        public long Id { get; set; }


        [Display(Name = "标题")]
        public string Title { get; set; }

        [Display(Name = "内容")]
        public string Content { get; set; }

        [Display(Name = "来源")]
        public string Source { get; set; }
        public bool HasDateRange { get; set; }

        [Display(Name = "发布分区")]
        public NewsAreaType? Area { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "发布日期")]

        public DateTime PublishDate { get; set; }
        [Display(Name = "失效日期")]
        [DataType(DataType.Date)]
        public DateTime? ExpireDate { get; set; }

        [Display(Name = "顺序")]
        public int? Order { get; set; }


        [Display(Name = "图片")]
        public IFormFile Image { get; set; }

        [Display(Name = "图片")]
        public string ImageBase64 { get; set; }


        public async Task FillNewsItem(NewsItem news)
        {
            news.Content = this.Content;
            news.Title = this.Title;
            news.Source = this.Source;
            news.Area = this.Area;
            news.PublishDate = this.PublishDate;
            news.ExpireDate = this.ExpireDate;
            news.Order = this.Order;

            if (Image != null)
            {
                news.Metadata = (await this.Image.GetPictureByteArray()).ToBase64String();
            }

        }

    }
}
