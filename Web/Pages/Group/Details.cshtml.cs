﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Infrastructure.Interfaces;
using Socona.ImVehicle.Web.ViewModels;

namespace Socona.ImVehicle.Web.Pages.Group
{
    public class DetailsModel : PageModel
    {
        private readonly VehicleDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserFileService _userFileService;
        public DetailsModel(VehicleDbContext context, IAuthorizationService authorizationService,
            IUserFileService userFileService)
        {
            _context = context;
            _authorizationService = authorizationService;
            _userFileService = userFileService;
        }

        public GroupDetailViewModel GroupItem { get; set; }


        public List<UserFileListViewModel> GlobalFiles { get; set; }
        public async Task<bool> CanEdit()
        {
            var tm = _authorizationService.AuthorizeAsync(HttpContext.User, GroupItem.OriginalModel, "CanEdit");
            return (await tm).Succeeded;
        }
        public async Task<bool> CanUpload()
        {
            var can = _authorizationService.AuthorizeAsync(HttpContext.User, GroupItem.OriginalModel, "CanUploadUserFile");
            var gm = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireGroupManagerRole");

            return (await gm).Succeeded && (await can).Succeeded;
        }


        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var group = await _context.Groups.Where(m => m.Id == id)
                .Include(t => t.Town)
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
            GroupItem = new GroupDetailViewModel(group);
            var gf = (await _userFileService.GetGlobalUserFilesAsync()).Select(t => new UserFileListViewModel(t)).ToList();
            var tf = (await _userFileService.GetUserFilesForTownAsync(group.TownId.Value)).Select(t => new UserFileListViewModel(t)).ToList();
            GlobalFiles = gf.Union(tf).ToList();

            return Page();
        }
    }
}
