using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Socona.ImVehicle.Core.Extensions
{
    public static class EnumExtension
    {
        public static string GetDisplayName<T>(this T enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                ?.First()
                ?.GetCustomAttribute<DisplayAttribute>()
                ?.GetName()??enumValue.ToString();
        }

        public static HtmlString EnumToHtmlString<T>(this IHtmlHelper helper) 
        {
            var values = Enum.GetValues(typeof(T)).Cast<T>();
            var enumDictionary = values.Select(v => new { name = GetDisplayName(v), value = v, });
            return new HtmlString(JsonConvert.SerializeObject(enumDictionary));
        }

    }
}
