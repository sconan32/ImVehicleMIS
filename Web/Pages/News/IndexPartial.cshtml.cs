using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.ViewModels;

namespace Web.Pages.News
{
    public class IndexPartialModel : PageModel
    {
        IAsyncRepository<NewsItem> _newsRepisitory;

        public IndexPartialModel(IAsyncRepository<NewsItem> newsRepisitory)
        {
            this._newsRepisitory = newsRepisitory;
        }

        public List<NewsListViewModel> NewsList { get; set; } = new List<NewsListViewModel>();


        public async Task OnGet()
        {
            var news = await _newsRepisitory.ListRangeAsync(0, 10);

            NewsList = news
                .Select(o => new NewsListViewModel()
                {
                    Id = o.Id,
                    Title = o.Title,
                    Date = o.PublishDate

                }).ToList();
        }
    }
}