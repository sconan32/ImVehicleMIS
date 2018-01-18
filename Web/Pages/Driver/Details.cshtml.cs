using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Web.ViewModels;

namespace Socona.ImVehicle.Web.Pages.Driver
{
    public class DetailsModel : PageModel
    {
        private readonly Socona.ImVehicle.Core.Data.VehicleDbContext _context;
        private readonly ITownRepository _townRepository;
        private readonly IAuthorizationService _authorizationService;

        IGroupRepository _groupService;
        public DetailsModel(Socona.ImVehicle.Core.Data.VehicleDbContext context, ITownRepository townRepository, IGroupRepository groupService, IAuthorizationService authorizationService)
        {
            _townRepository = townRepository;
            _groupService = groupService;
            _authorizationService = authorizationService;
            _context = context;
        }
        public async Task<bool> CanEdit()
        {
            var tm = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireTownManagerRole");
            var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await tm).Succeeded || (await admin).Succeeded;
        }

        public DriverDetailViewModel DriverItem { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .Include(t => t.Vehicles).ThenInclude(v => v.Town)
                .Include(t => t.Vehicles).ThenInclude(v => v.Group)
                .Include(t => t.Town)
                .Include(t => t.Group)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (driver == null)
            {
                return NotFound();
            }
            DriverItem = new DriverDetailViewModel()
            {
                Id = driver.Id,
                Name = driver.Name,
                Gender = driver.Gender,
                FirstLicenseIssueDate = driver.FirstLicenseIssueDate,
                LicenseIssue = driver.LicenseIssueDate,

                IdCardNumber = driver.IdCardNumber,
                License = driver.LicenseNumber,
                LicenseType = driver.LicenseType,
                ValidYears = driver.LicenseValidYears,

                LivingAddress = driver.LivingAddress,
                Tel = driver.Tel,
                Title = driver.Title,
                WarrantyCode = driver.WarrantyCode,
                ResidentType=driver.ResidentType,

                TownName = driver.Town?.Name,
                GroupName = driver.Group?.Name,
                IsValid = driver.IsValid(),

                PhotoDriverLicenseBase64 = driver.PhotoDriverLicense != null ? Convert.ToBase64String(driver.PhotoDriverLicense) : "",
                PhotoIdCard1Base64 = driver.PhotoIdCard1 != null ? Convert.ToBase64String(driver.PhotoIdCard1) : "",
                PhotoIdCard2Base64 = driver.PhotoIdCard2 != null ? Convert.ToBase64String(driver.PhotoIdCard2) : "",
                PhotoWarrantyBase64 = driver.PhotoWarranty != null ? Convert.ToBase64String(driver.PhotoWarranty) : "",




                Vehicles = driver.Vehicles.Select(t => new VehicleListViewModel(t)).ToList(),
                VehiclesRegistered = driver.Vehicles.Count,
            };


            return Page();
        }
    }
}
