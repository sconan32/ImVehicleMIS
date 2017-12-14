using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;

namespace Socona.ImVehicle.Core.Services
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
