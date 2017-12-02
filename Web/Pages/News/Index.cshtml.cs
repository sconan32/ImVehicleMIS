using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ImVehicleCore.Data;
using Microsoft.AspNetCore.Authorization;
using Web.ViewModels;

namespace Web.Pages.News
{
    public class IndexModel : PageModel
    {
        private readonly ImVehicleCore.Data.VehicleDbContext _context;

        private readonly IAuthorizationService _authorizationService;

        public IndexModel(ImVehicleCore.Data.VehicleDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }
        public async Task<bool> CanEdit()
        {
            var tm = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireGlobalVisitorRole");
            var admin = _authorizationService.AuthorizeAsync(HttpContext.User, "RequireAdminsRole");
            return (await tm).Succeeded || (await admin).Succeeded;
        }
        public IList<NewsListViewModel> Newses { get; set; }

        public async Task OnGetAsync()
        {
            var ns = await _context.Newses.ToListAsync();
            Newses = ns.Select(t => new NewsListViewModel()
            {
                Id = t.Id,
                Order = t.Order,
                Date = t.PublishDate,
                Title = t.Title

            }).ToList();
        }
    }
}
