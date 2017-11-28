using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImVehicleCore.Data;
using System.ComponentModel.DataAnnotations;

namespace Web.Pages.Group
{
    public class DetailsModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;

        public DetailsModel(ImVehicleCore.Data.VehicleDbContext context)
        {
            _context = context;
        }

        public GroupViewModel GroupItem { get; set; }


        public class GroupViewModel
        {
            public long Id { get; set; }
            [Display(Name = "名称")]
            public string Name { get; set; }
            [Display(Name = "办公地址")]
            public string Address { get; set; }
            [Display(Name = "注册地址")]
            public string RegisterAddress { get; set; }
            [Display(Name = "注册号")]
            public string License { get; set; }

            [Display(Name = "负责人")]
            public string ChiefName { get; set; }
            [Display(Name = "负责人电话")]
            public string ChiefTel { get; set; }

            [Display(Name = "单位类型")]
            public string Type { get; set; }

            [Display(Name = "企业图像")]
            public string PhotoMain { get; set; }
            [Display(Name = "资质凭证")]
            public string PhotoWarranty { get; set; }
            [Display(Name = "安全生产凭证")]
            public string PhotoSecurity { get; set; }

            [Display(Name = "注册车辆数目")]
            public int VehicleCount { get; set; }
            [Display(Name = "   其中：处于正常状态")]
            public int ValidCount { get; set; }
            [Display(Name = "       处于预警状态")]
            public int InvalidCount { get; set; }

            public List<VehicleListViewModel> Vehicles { get; set; }

        }
        public class VehicleListViewModel
        {
            public long Id { get; set; }
            [Display(Name = "车牌号")]
            public string License { get; set; }
            [Display(Name = "品牌")]
            public string Brand { get; set; }
            [Display(Name = "型号")]
            public string Name { get; set; }
            [Display(Name = "类型")]
            public VehicleType Type { get; set; }
            [Display(Name = "颜色")]
            public string Color { get; set; }
            [Display(Name = "注册时间")]
            public DateTime LastRegisterDate { get; set; }
        }
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var group = await _context.Groups.Include(t => t.Vehicles).SingleOrDefaultAsync(m => m.Id == id);
            GroupItem = new GroupViewModel()
            {
                Id = group.Id,
                Name = group.Name,
                Address = group.Address,
                RegisterAddress = group.RegisterAddress,
                License = group.License,
                ChiefName = group.ChiefName,
                ChiefTel = group.ChiefTel,
                Type = group.Type,
                PhotoMain = group.PhotoMain != null ? Convert.ToBase64String(group.PhotoMain) : "",
                PhotoWarranty = group.PhotoWarranty != null ? Convert.ToBase64String(group.PhotoWarranty) : "",
                PhotoSecurity = group.PhotoSecurity != null ? Convert.ToBase64String(group.PhotoSecurity) : "",


                VehicleCount = group.Vehicles.Count,
                InvalidCount = group.Vehicles.Count(v => v.RegisterDate.Date.AddYears(1) < DateTime.Now.Date),
                ValidCount = group.Vehicles.Count(v => v.RegisterDate.Date.AddYears(1) >= DateTime.Now.Date),

                Vehicles = group.Vehicles.Select(t => new VehicleListViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Brand = t.Brand,
                    Color = t.Color,
                    License = t.LicenceNumber,
                    LastRegisterDate = t.RegisterDate,
                    Type = t.Type,
                }).ToList(),
            };

            if (GroupItem == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
