using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Socona.ImVehicle.Web.Pages
{
    public class AboutModel : PageModel
    {
        public string Message { get; set; }

        public List<string> Updates { get; set; }

        public List<string> KnownIssues { get; set; }
        public void OnGet()
        {
            Message = "Your application description page.";

            Updates = new List<string>()
            {
                "发布日期：2018/5/26",
                "[!] 修正了gjz用户登录时不能获取数据的问题",
                "[!] 修正了从详情页删除项时会跳转到404页面的问题",
                "[!] 现在使用MySQL数据库，防止出现多人访问数据库被锁定的问题",
                "[!] 修正了个别表单页面上传图片不正确问题"


            };

        }
    }
}
