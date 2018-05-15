using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Core.Specifications;

namespace Socona.ImVehicle.Core.Services
{
    public class NewsService : INewsService
    {
        private readonly IAsyncRepository<NewsItem> _entityRepository;

        public NewsService(IAsyncRepository<NewsItem> entityRepository)
        {
            _entityRepository = entityRepository;
        }



        public async Task<List<NewsItem>> LoadLoginNews()
        {
            Specification<NewsItem> spec = new Specification<NewsItem>(t => t.Area == NewsAreaType.Notification);
            return await _entityRepository.ListRangeAsync(spec, 0, 10);
        }

        public async Task<List<NewsItem>> LoadLoginCases()
        {
            Specification<NewsItem> spec = new Specification<NewsItem>(t => t.Area == NewsAreaType.AccidentCase);
            return await _entityRepository.ListRangeAsync(spec, 0, 10);
        }

        public async Task<List<NewsItem>> LoadLoginLaws()
        {
            Specification<NewsItem> spec = new Specification<NewsItem>(t => t.Area == NewsAreaType.LawAndRule);
            return await _entityRepository.ListRangeAsync(spec, 0, 10);
        }


        public async Task<List<NewsItem>> LoadLoginImages()
        {
            Specification<NewsItem> spec = new Specification<NewsItem>(t => t.Area == NewsAreaType.ImageNews);
            var newses = await _entityRepository.ListRangeAsync(spec, 0, 3);
            for (int i = newses.Count; i < 3; i++)
            {
                newses.Add(new NewsItem() { Name = "", Metadata = "" });
            }
            return newses;
            
        }






    }
}
