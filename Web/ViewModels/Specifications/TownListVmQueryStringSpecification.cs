using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ImVehicleCore.Specifications;

namespace Web.ViewModels.Specifications
{
    public class TownListVmQueryStringSpecification : QueryStringSpecificationBase<TownItemListViewModel>
    {
        public TownListVmQueryStringSpecification(string queryString) : base(queryString) { }



        protected override Dictionary<string, Tuple<string, Type>> GetNamePropertyMap()
        {
            Dictionary<string, Tuple<string, Type>> dictionary = new Dictionary<string, Tuple<string, Type>>();

            foreach (var propInfo in typeof(TownItemListViewModel).GetProperties())
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
                        dictionary[(name as DisplayAttribute).Name] = new Tuple<string, Type>(prop, type);
                    }
                }
            }


            return dictionary;
        }

    }
}
