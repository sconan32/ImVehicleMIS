using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Socona.ImVehicle.Core.Data
{
    public class DistrictItem : BaseEntity
    {
        [MaxLength(2048)]
        public string Address { get; set; }

        public AdminDivisionType DivisionType { get; set; }
        public virtual List<TownItem> Towns { get; set; }

        public virtual List<VehicleUser> Users { get; set; }

    }

    public enum AdminDivisionType
    {
        [Display(Name ="国家")]
        Country=0,
        [Display(Name = "地区")]
        SubCountry =1,
        [Display(Name = "省")]
        Province =2,
        [Display(Name = "省区")]
        SubProvince =3,
        [Display(Name = "市")]
        Prefecture =4,
        [Display(Name = "地区")]
        SubPrefecture =5,
        [Display(Name = "县/市")]
        County =6,
        [Display(Name = "街道/乡/镇")]
        Township =7,
        [Display(Name = "村")]
        Village =8,
        

    }
}
