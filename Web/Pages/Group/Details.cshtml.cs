﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;
using System.ComponentModel.DataAnnotations;
using Web.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.Group
{
    public class DetailsModel : PageModel
    {
        private readonly VehicleDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        public DetailsModel(VehicleDbContext context, IAuthorizationService authorizationService)
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
                .Include(t=>t.Town)
                .Include(t => t.Vehicles)
                .Include(t => t.Drivers).ThenInclude(d => d.Vehicles)
                .Include(t => t.UserFiles)
                .Include(t => t.SecurityPersons).ThenInclude(s => s.Group)
                .Include(t => t.SecurityPersons).ThenInclude(s => s.Town)
                .SingleOrDefaultAsync();
            if (group == null)
            {
                return NotFound();
            }
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
                
                TownName = group.Town.Name,
                VehicleCount = group.Vehicles.Count,

                DriverCount = group.Drivers.Count,
                SecuremanCount = group.Drivers.Count,
                DriverInvalidCount = group.Drivers.Count(d => !d.IsValid()),
                VehicleInvalidCount = group.Vehicles.Count(v => !v.IsValid()),
                IsValid = group.IsValid(),


                Vehicles = group.Vehicles.Select(t => new VehicleListViewModel(t)).ToList(),
                Drivers = group.Drivers.Select(t => new DriverListViewModel(t)).ToList(),
                UserFiles = group.UserFiles.Select(t => new UserFileListViewModel()
                {
                    Id = t.Id,
                    FileName = t.FileName,
                    ServerPath = t.ServerPath,
                    GroupName = group.Name,
                    Name = t.Name,
                    Size = t.Size,
                    UploadDate=t.CreationDate,

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
         
      
            

            return Page();
        }
    }
}
