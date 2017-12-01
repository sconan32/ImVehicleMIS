using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImVehicleCore.Data;
using System.ComponentModel.DataAnnotations;
using Web.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.Group
{
    public class DetailsModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        public DetailsModel(ImVehicleCore.Data.VehicleDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        public GroupDetailViewModel GroupItem { get; set; }

        public async Task<bool> CanEdit()
        {
            var tm = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireTownManagerRole");
            var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await tm).Succeeded || (await admin).Succeeded;
        }



        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var group = await _context.Groups.Where(m => m.Id == id)
                .Include(t => t.Vehicles)
                .Include(t => t.Drivers).ThenInclude(d => d.Vehicles)
                .Include(t => t.UserFiles)
                .Include(t => t.SecurityPersons).ThenInclude(s => s.Group)
                .Include(t => t.SecurityPersons).ThenInclude(s => s.Town)
                .SingleOrDefaultAsync();
            GroupItem = new GroupDetailViewModel()
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
                Drivers = group.Drivers.Select(t => new DriverListViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    IdCardNumber = t.IdCardNumber,
                    License = t.LicenseNumber,
                    LicenseType = t.LicenseType,
                    LicenseIssue = t.LicenseIssueDate,
                    ValidYears = t.LicenseValidYears,
                    Gender = t.Gender,
                    VehiclesRegistered = t.Vehicles?.Count ?? 0,
                    Tel = t.Tel,

                }).ToList(),

                UserFiles = group.UserFiles.Select(t => new UserFileListViewModel()
                {
                    Id = t.Id,
                    FileName = t.FileName,
                    ServerPath = t.ServerPath,
                    GroupName = group.Name,
                    Name = t.Name,
                    Size = t.Size,

                }).ToList(),

                Securemans = group.SecurityPersons.Select(t => new SecureManListViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Address = t.Address,
                    RegisterAddress = t.RegisterAddress,
                    Company = t.Company,
                    GroupName = t.Group?.Name,
                    TownName = t.Town?.Name,
                    IdCardNum = t.IdCardNum,
                    Tel = t.Tel,
                    Title = t.Title,
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
