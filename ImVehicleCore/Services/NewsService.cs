using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImVehicleCore.Data;
using ImVehicleCore.Interfaces;

namespace ImVehicleCore.Data
{
    public class NewsService : INewsService
    {
        private readonly IAsyncRepository<NewsItem> _entityRepository;

        public NewsService(IAsyncRepository<NewsItem> entityRepository)
        {
            _entityRepository = entityRepository;
        }

       

       public Task<List<NewsItem>> LoadLoginNews ()
        {

            return _entityRepository.ListRangeAsync(0, 10);
        }

 

      

     

       
       
     
    }
}
