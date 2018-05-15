using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Core.Interfaces
{
    public interface INewsService
    {

        Task<List<NewsItem>> LoadLoginNews();
        Task<List<NewsItem>> LoadLoginCases();

        Task<List<NewsItem>> LoadLoginLaws();

        Task<List<NewsItem>> LoadLoginImages();
    }
}
