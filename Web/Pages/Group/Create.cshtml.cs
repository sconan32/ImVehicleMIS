using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImVehicleCore.Data;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.Group
{
    public class CreateModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;
        private readonly SignInManager<VehicleUser> _signInManager;

        public CreateModel(ImVehicleCore.Data.VehicleDbContext context,SignInManager<VehicleUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }
       
        [Authorize]
        public IActionResult OnGet()
        {                      
                return Page();            
          
        }

        [BindProperty]
        public GroupViewModel GroupItem { get; set; }

        [Authorize]
        public async Task<IActionResult> OnPostAsync()
        {           
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user =await _signInManager.UserManager.GetUserAsync(HttpContext.User);
          

            MemoryStream mainPhotoS = new MemoryStream();
            MemoryStream warPhotoS = new MemoryStream();
            MemoryStream  secPhotoS =new MemoryStream();
          
            await GroupItem.PhotoMain.CopyToAsync(mainPhotoS);
            await GroupItem.PhotoSecurity.CopyToAsync(secPhotoS);
            await GroupItem.PhotoWarranty.CopyToAsync(warPhotoS);
            var group = new GroupItem()
            {
                Name = GroupItem.Name,
                Address = GroupItem.Address,
                RegisterAddress = GroupItem.RegisterAddress,
                License = GroupItem.License,
                ChiefName = GroupItem.ChiefName,
                ChiefTel = GroupItem.ChiefTel,
                Type = GroupItem.Type,
                TownId = user?.TownId,
                PhotoMain = mainPhotoS.ToArray(),
                PhotoWarranty = warPhotoS.ToArray(),
                PhotoSecurity = warPhotoS.ToArray(),
            };

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public class GroupViewModel
        {
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

            [Display(Name="单位类型")]
            public string Type { get; set; }

            [Display(Name = "企业图像")]
            public IFormFile PhotoMain { get; set; }
            [Display(Name = "资质凭证")]
            public IFormFile PhotoWarranty { get; set; }
            [Display(Name = "安全生产凭证")]
            public IFormFile PhotoSecurity { get; set; }
        }


    }
}