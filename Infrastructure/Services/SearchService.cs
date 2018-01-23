using Socona.ImVehicle.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using Socona.SearchParser;
using Socona.ImVehicle.Web.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Socona.SearchParser.Parser;

namespace Socona.ImVehicle.Infrastructure.Services
{
    public class SearchService : ISearchService
    {

        public SearchService()
        {
            _parserContext = new SearchParserContext();
            var rootMap = new PropertyMap();
            rootMap.Add(new PropertyItem { Source = "街道", Target = "TownListViewModel", TargetType = typeof(TownListViewModel) });
            rootMap.Add(new PropertyItem { Source = "镇", Target = "TownListViewModel", TargetType = typeof(TownListViewModel) });
            rootMap.Add(new PropertyItem { Source = "安全单位", Target = "GroupListViewModel", TargetType = typeof(GroupListViewModel) });
            rootMap.Add(new PropertyItem { Source = "单位", Target = "GroupListViewModel", TargetType = typeof(GroupListViewModel) });
            rootMap.Add(new PropertyItem { Source = "公司", Target = "GroupListViewModel", TargetType = typeof(GroupListViewModel) });
            rootMap.Add(new PropertyItem { Source = "村", Target = "GroupListViewModel", TargetType = typeof(GroupListViewModel) });
            rootMap.Add(new PropertyItem { Source = "驾驶员", Target = "DriverListViewModel", TargetType = typeof(DriverListViewModel) });
            rootMap.Add(new PropertyItem { Source = "司机", Target = "DriverListViewModel", TargetType = typeof(DriverListViewModel) });
            rootMap.Add(new PropertyItem { Source = "车辆", Target = "VehicleListViewModel", TargetType = typeof(VehicleListViewModel) });
            rootMap.Add(new PropertyItem { Source = "车", Target = "VehicleListViewModel", TargetType = typeof(VehicleListViewModel) });
            
            _parserContext.Properties[""] = rootMap;
            _parserContext.Properties["TownListViewModel"] = ParsePropertyOfType(typeof(TownListViewModel));
            _parserContext.Properties["GroupViewModel"] = ParsePropertyOfType(typeof(GroupViewModel));
            _parserContext.Properties["DriverListViewModel"] = ParsePropertyOfType(typeof(DriverListViewModel));
            _parserContext.Properties["VehicleListViewModel"] = ParsePropertyOfType(typeof(VehicleListViewModel));
        }



        SearchParserContext _parserContext;
        
        private PropertyMap  ParsePropertyOfType(Type targetType)
        {
            PropertyMap map = new PropertyMap();

            foreach (var propInfo in targetType.GetProperties())
            {
                var type = propInfo.PropertyType;
                var prop = propInfo.Name;
                map.Add(new PropertyItem { Source = prop, Target = prop, TargetType = type });
                var dd = propInfo.GetCustomAttributes(typeof(DisplayAttribute), true);
                if (dd.Length > 0)
                {
                    var name = dd.FirstOrDefault(a => a is DisplayAttribute);
                    if (name != null)
                    {
                        map.Add(new PropertyItem { Source = (name as DisplayAttribute).Name, Target = prop, TargetType = type });
                    }
                }
            }

            return map;
        }

        public bool BuildSearchExpression(string queryString,out string urlPath,out Expression expression)
        {
            Interpreter interpreter = new Interpreter(_parserContext);
            urlPath = MapUrl(interpreter.FirstDeductedType);
            expression= interpreter.GetExpression(queryString);
            return true;
        }

        private string MapUrl(Type type)
        {
            if (type == typeof(GroupListViewModel))
            {
                return "Group";
            }
            else if (type == typeof(VehicleListViewModel))
            {
                return "Vehicle";
            }
            else if (type == typeof(DriverListViewModel))
            {
                return "Driver";
            }
            else return "";
        }
    }
}
