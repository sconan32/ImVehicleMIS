using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Core.Specifications
{
   public class TownQueryStringSpecification : QueryStringSpecification<TownItem>
    {
        public TownQueryStringSpecification(string queryString) : base(queryString) { }



        protected override Dictionary<string, Tuple<string, Type>> GetNamePropertyMap()
        {
            Dictionary<string, Tuple<string, Type>> dictionary = new Dictionary<string, Tuple<string, Type>>();

            foreach (var propInfo in typeof(TownItem).GetProperties())
            {
                var type = propInfo.PropertyType;
                var prop = propInfo.Name;
                dictionary.Add(prop, new Tuple<string, Type>(prop, type));
                var dd = propInfo.GetCustomAttributes(typeof(DisplayAttribute), true);
                if (dd.Length > 0)
                {
                    var name = dd.FirstOrDefault(a => a is DisplayAttribute);
                    if (name != null)
                    {
                        dictionary[(name as DisplayAttribute).Name]= new Tuple<string, Type>(prop, type);
                    }
                }
            }


            return dictionary;
        }

    }
}
