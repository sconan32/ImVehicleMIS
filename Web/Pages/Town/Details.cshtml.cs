using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImVehicleCore.Data;
using ImVehicleCore.Interfaces;
using System.ComponentModel.DataAnnotations;
using Web.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.Towns
{
    public class DetailsModel : PageModel
    {

        private readonly ITownRepository _townRepository;
        private readonly IAuthorizationService _authorizationService;

        IGroupRepository _groupService;
        public DetailsModel(ITownRepository townRepository, IGroupRepository groupService,IAuthorizationService authorizationService)
        {
            _townRepository = townRepository;
            _groupService = groupService;
            _authorizationService = authorizationService;
        }

        public TownDetailViewModel TownItem { get; set; }


        public async Task<bool> CanEdit()
        {
            var tm=  _authorizationService.AuthorizeAsync(HttpContext.User, "RequireTownManagerRole");
            var admin=  _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await tm).Succeeded || (await admin).Succeeded;
        }


        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var town = await _townRepository.GetByIdEagerAsync(id.Value);
            TownItem = new TownDetailViewModel()
            {
                Id = town.Id,
                Name = town.Name,
                GroupCount = town.Groups.Count,
                ValidCount = town.Groups.Count(t => t.Vehicles.Any(v => v.RegisterDate.Date.AddYears(1) >= DateTime.Now.Date)),
                InvalidCount = town.Groups.Count(t => t.Vehicles.Any(v => v.RegisterDate.Date.AddYears(1) < DateTime.Now.Date)),
                Groups = town.Groups.Select(t => new GroupListViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    ChiefName = t.ChiefName,
                    ChiefTel = t.ChiefTel,
                    ChiefTitle = t.ChiefTitle,
                    Address = t.Address,
                    License = t.License,
                    Type = t.Type,
                    VehicleCount = t.Vehicles.Count,
                    InvalidCount = t.Vehicles.Count(v => v.RegisterDate.Date.AddYears(1) < DateTime.Now.Date)
                }).ToList(),
                Drivers = town.Drivers.Select(t => new DriverListViewModel()
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
                Vehicles = town.Groups.SelectMany(g => g.Vehicles).Select(t => new VehicleListViewModel()
                {
                    Id = t.Id,
                    License = t.LicenceNumber,
                    Name = t.Name,
                    Brand = t.Brand,
                    Type = t.Type,
                    Color = t.Color,
                    LastRegisterDate = t.RegisterDate,
                    GroupName = t.Group?.Name,
                    TownName = t.Group?.Town?.Name,
                    DriverName = t?.Driver?.Name,
                    DriverTel = t?.Driver?.Tel,
                }).ToList(),
            };

            if (TownItem == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
