using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Data;

namespace Socona.ImVehicle.Web.ViewModels
{
    public class VehicleListViewModel
    {

        public VehicleListViewModel(VehicleItem t = null)
        {
          
            if (t != null)
            {
                Id = t.Id;
                Name = t.Name;
                Brand = t.Brand;
                Color = t.Color;
                License = t.LicenceNumber;
                LastRegisterDate = t.LastRegisterDate;
                YearlyAuditDate = t.YearlyAuditDate;
                AuditExpiredDate = t.AuditExpiredDate;
                DriverName = t.Driver?.Name;
                TownName = t.Town?.Name;
                GroupName = t.Group?.Name;
                Type = t.Type;
                Usage = t.Usage;
                IsValid = t.IsValid();
                DumpDate = t.DumpDate;


            }
        }
        public long Id { get; set; }
        [Display(Name = "车牌号")]
        public string License { get; set; }

        [Display(Name = "类型")]
        public VehicleType Type { get; set; }

        [Display(Name = "营运方式")]
        public UsageType Usage { get; set; }
        [Display(Name = "品牌")]
        public string Brand { get; set; }
        [Display(Name = "型号")]
        public string Name { get; set; }

        [Display(Name = "颜色")]
        public string Color { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "注册时间")]
        public DateTime? LastRegisterDate { get; set; }
        [Display(Name = "安全单位")]
        public string GroupName { get; set; }
        [Display(Name = "街道")]
        public string TownName { get; set; }
        [Display(Name = "驾驶员")]
        public string DriverName { get; set; }
        [Display(Name = "驾驶员电话")]
        public string DriverTel { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "年检日期")]
        public DateTime? YearlyAuditDate { get; set; }
        public bool IsAuditValid { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "报废日期")]
        public DateTime? DumpDate { get; set; }

        public bool IsDumpDateValid { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "检验有效期至")]
        public DateTime? AuditExpiredDate { get; set; }
        public bool IsInsuranceValid { get; set; }


        public bool IsValid { get; private set; }
    }
}
