using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Socona.ImVehicle.Web.Pages
{
    public class ErrorModel : PageModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);


      public string Code { get; set; }

        public void OnGet(string code  )
        {

            Code = code;
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}
