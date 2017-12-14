using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Socona.ImVehicle.Infrastructure.Interfaces
{
  public  interface ISearchService
    {


        bool BuildSearchExpression(string queryString, out string urlPath, out Expression expression);

    }
}
