using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImVehicleCore.Data;

namespace ImVehicleCore.Interfaces
{
    public interface INewsService
    {

        Task<List<NewsItem>> LoadLoginNews();
    }
}
