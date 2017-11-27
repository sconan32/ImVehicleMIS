using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImVehicleCore.Data;
using ImVehicleCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static ImVehicleMIS.Pages.Account.LoginModel;

namespace ImVehicleMIS.Pages
{
    public class IndexModel : PageModel
    {
        IAsyncRepository<NewsItem> _newsRepisitory;

        public IndexModel(IAsyncRepository<NewsItem> newsRepisitory)
        {
            this._newsRepisitory = newsRepisitory;
        }
      public  InputModel Input { get; set; }
        public List<NewsListView> NewsList { get; set; } = new List<NewsListView>();
        public class NewsListView
        {
            public long Id { get; set; }

            public string Name { get; set; }

            public DateTime Date { get; set; }
        }

        public async Task OnGet()
        {
            var news = await _newsRepisitory.ListRangeAsync(0, 10);

            NewsList = news
                .Select(o => new NewsListView()
                {
                    Id = o.Id,
                    Name = o.Name,
                    Date = o.PublishDate

                }).ToList();
        }
    }
}
