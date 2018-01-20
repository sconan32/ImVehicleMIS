using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Core.Specifications;
using Socona.ImVehicle.Web.ViewModels;
using Socona.ImVehicle.Infrastructure.Extensions;
using System.IO;
using NPOI.SS.Util;
using System.Security.Cryptography;
using System.Text;

namespace Socona.ImVehicle.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class DriverController : Controller
    {


        private readonly VehicleDbContext _context;
        private readonly UserManager<VehicleUser> _userManager;
        private IAsyncRepository<DriverItem> _driverPepository;

        public DriverController(VehicleDbContext context, UserManager<VehicleUser> userManager, IAsyncRepository<DriverItem> driverPepository)
        {
            _context = context;
            _userManager = userManager;
            _driverPepository = driverPepository;
        }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> GetDropDownList(long? townId, long? groupId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            IEnumerable<DriverItem> drivers = new List<DriverItem>();
            if (await _userManager.IsInRoleAsync(user, "TownManager"))
            {
                if (user.TownId == townId)
                {
                    drivers = _context.Drivers.Where(t => t.TownId == townId || t.GroupId == groupId);
                }

            }
            else
            {
                drivers = _context.Drivers.Where(t => t.TownId == townId || t.GroupId == groupId);
            }

            var list = drivers.Select(t => new { Value = t.Id, Text = (t.GroupId == groupId ? "*" : "") + t.Name + " (" + t.IdCardNumber + ")" });
            return new JsonResult(list);

        }

        [Authorize(Roles = "GlobalVisitor,TownManager,Admins")]
        public async Task<IActionResult> LoadData(int? townId, int? page = 0, int? pageSize = 20)
        {
            ISpecification<DriverItem> canFetch = await Driver4UserSpecification.CreateAsync(HttpContext.User, _userManager);


            if (townId != null)
            {
                var inGroup = new DriverInTownSpecification(townId.Value);
                canFetch = canFetch.And(inGroup);
            }
            canFetch.Includes.Add(t => t.Vehicles);
            canFetch.Includes.Add(t => t.Town);
            canFetch.Includes.Add(t => t.Group);

            var startIdx = (page) * pageSize ?? 0;
            startIdx = Math.Max(0, startIdx);
            var groups = await _driverPepository.ListRangeAsync(canFetch, startIdx, pageSize ?? 0);
            var list = groups.Select(t => new DriverListViewModel(t));
            return new JsonResult(list);
        }

        [Authorize(Roles = "GlobalVisitor,TownManager,Admins")]
        public async Task<IActionResult> ExportExcel(long? townId, long? groupId)
        {
            ISpecification<DriverItem> canFetch = await Driver4UserSpecification.CreateAsync(HttpContext.User, _userManager);


            if (townId != null)
            {
                var inTown = new DriverInTownSpecification(townId.Value);
                canFetch = canFetch.And(inTown);
            }
            if (groupId != null)
            {
                var inGroup = new Specification<DriverItem>(t => t.GroupId == groupId);
                canFetch = canFetch.And(inGroup);
            }
            canFetch.Includes.Add(t => t.Vehicles);
            canFetch.Includes.Add(t => t.Town);
            canFetch.Includes.Add(t => t.Group);

            var items = await _driverPepository.ListAsync(canFetch);
            items= items.OrderBy(t => t.IsValid()).ThenBy(t => t.LicenseIssueDate).ToList();
            if (!items.Any())
            {
                return NoContent();
            }

            XSSFWorkbook workbook = new XSSFWorkbook();
            ISheet sheet1 = workbook.CreateSheet($"驾驶员列表");
            sheet1.PrintSetup.Landscape = true;
            sheet1.SetMargin(MarginType.RightMargin, 0.3d);
            sheet1.SetMargin(MarginType.TopMargin, 0.3d);
            sheet1.SetMargin(MarginType.LeftMargin, 0.3d);
            sheet1.SetMargin(MarginType.BottomMargin, 0.3d);
            sheet1.PrintSetup.PaperSize = (short)PaperSize.A4 + 1;

            IFont fontTitle = workbook.CreateFont();
            fontTitle.FontName = "黑体";
            fontTitle.FontHeight = 18;

            ICellStyle styTitle = workbook.CreateCellStyle();
            styTitle.SetFont(fontTitle);
            styTitle.FillPattern = FillPattern.SolidForeground;
            styTitle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;


            IFont fontContent = workbook.CreateFont();
            fontContent.FontName = "等线";
            fontContent.FontHeight = 10;

            ICellStyle styContent = workbook.CreateCellStyle();
            styContent.SetFont(fontContent);
            styContent.BorderBottom = BorderStyle.Thin;
            styContent.BottomBorderColor = IndexedColors.Grey50Percent.Index;
            styContent.BorderLeft = BorderStyle.Thin;
            styContent.LeftBorderColor = IndexedColors.Grey50Percent.Index;
            styContent.BorderRight = BorderStyle.Thin;
            styContent.RightBorderColor = IndexedColors.Grey50Percent.Index;
            styContent.BorderTop = BorderStyle.Thin;
            styContent.TopBorderColor = IndexedColors.Grey50Percent.Index;
            styContent.ShrinkToFit = true;

            IFont fontHeader = workbook.CreateFont();
            fontHeader.FontName = "等线";
            fontHeader.FontHeight = 10;
            fontHeader.IsBold = true;

            ICellStyle styHeader = workbook.CreateCellStyle();
            styHeader.SetFont(fontHeader);
            styHeader.BorderBottom = BorderStyle.Thin;
            styHeader.BottomBorderColor = IndexedColors.Grey50Percent.Index;
            styHeader.BorderLeft = BorderStyle.Thin;
            styHeader.LeftBorderColor = IndexedColors.Grey50Percent.Index;
            styHeader.BorderRight = BorderStyle.Thin;
            styHeader.RightBorderColor = IndexedColors.Grey50Percent.Index;
            styHeader.BorderTop = BorderStyle.Thin;
            styHeader.TopBorderColor = IndexedColors.Grey50Percent.Index;
            styHeader.FillPattern = FillPattern.SolidForeground;
            styHeader.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            styHeader.WrapText = true;

            int rowIdx = 0;
            int colIdx = 0;



            sheet1.CreateRow(rowIdx).CreateCell(colIdx).SetCellValue($"驾驶员列表");
            sheet1.GetRow(rowIdx).GetCell(0).CellStyle = styTitle;
            sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 12));


            rowIdx += 2;
            colIdx = 0;
            var rowSummary = sheet1.CreateRow(rowIdx);

            rowSummary.CreateCell(colIdx).SetCellValue($"共 {items.Count} 条记录 ");


            rowIdx += 1;
            colIdx = 0;
            var rowHeader = sheet1.CreateRow(rowIdx);
            rowHeader.Height = 70 * 10;
            rowHeader.CreateCell(colIdx++).SetCellValue("编号");
            rowHeader.CreateCell(colIdx++).SetCellValue("姓名");
            rowHeader.CreateCell(colIdx++).SetCellValue("性别");
            rowHeader.CreateCell(colIdx++).SetCellValue("电话");
            rowHeader.CreateCell(colIdx++).SetCellValue("身份证号");
            rowHeader.CreateCell(colIdx++).SetCellValue("居住地");
            rowHeader.CreateCell(colIdx++).SetCellValue("证照类型");
            rowHeader.CreateCell(colIdx++).SetCellValue("有效年限");
            rowHeader.CreateCell(colIdx++).SetCellValue("有效期至");
            rowHeader.CreateCell(colIdx++).SetCellValue("状态");
            rowHeader.CreateCell(colIdx++).SetCellValue("注册车辆");
            rowHeader.CreateCell(colIdx++).SetCellValue("街道");
            rowHeader.CreateCell(colIdx++).SetCellValue("安全组");

            sheet1.CreateFreezePane(0, 4);

            rowHeader.Cells.ForEach(t => { t.CellStyle = styHeader; });



            sheet1.SetColumnWidth(0, 120 * 10);
            sheet1.SetColumnWidth(1, 210 * 10);
            sheet1.SetColumnWidth(2, 90 * 10);
            sheet1.SetColumnWidth(3, 355 * 10);
            sheet1.SetColumnWidth(4, 505 * 10);
            sheet1.SetColumnWidth(5, 140 * 10);
            sheet1.SetColumnWidth(6, 130 * 10);
            sheet1.SetColumnWidth(7, 130 * 10);
            sheet1.SetColumnWidth(8, 270 * 10);
            sheet1.SetColumnWidth(9, 130 * 10);
            sheet1.SetColumnWidth(10, 130 * 10);
            sheet1.SetColumnWidth(11, 350 * 10);
            sheet1.SetColumnWidth(12, 700 * 10);



            for (int i = 0; i < items.Count; i++)
            {
                rowIdx += 1;
                colIdx = 0;
                IRow row = sheet1.CreateRow(rowIdx);
                row.Height = 32 * 10;
                row.CreateCell(colIdx++).SetCellValue(i + 1);
                row.CreateCell(colIdx++).SetCellValue(items[i].Name);
                row.CreateCell(colIdx++).SetCellValue(items[i].Gender.GetDisplayName());
                row.CreateCell(colIdx++).SetCellValue(items[i].Tel);
                row.CreateCell(colIdx++).SetCellValue(items[i].IdCardNumber);
                row.CreateCell(colIdx++).SetCellValue(items[i].ResidentType.GetDisplayName());
                row.CreateCell(colIdx++).SetCellValue(items[i].LicenseType.GetDisplayName());
                row.CreateCell(colIdx++).SetCellValue(items[i].LicenseValidYears?.ToString());
                row.CreateCell(colIdx++).SetCellValue(items[i].LicenseIssueDate?.ToShortDateString());
                row.CreateCell(colIdx++).SetCellValue(items[i].IsValid() ? "正常" : "预警");
                row.CreateCell(colIdx++).SetCellValue(items[i].Vehicles?.Count ?? 0);
                row.CreateCell(colIdx++).SetCellValue(items[i].Town?.Name);
                row.CreateCell(colIdx++).SetCellValue(items[i].Group?.Name);
                row.Cells.ForEach(t => { t.CellStyle = styContent; });
            }


            rowIdx += 1;
            colIdx = 0;
            var rowInfo = sheet1.CreateRow(rowIdx);
            var sigStr = $"::{HttpContext.User.Identity.Name}::socona.imvehicle.driver.export?town={townId}&group={groupId}::{DateTime.Now.ToString("yyyyMMdd.HHmmss")}";
            var sha1 = SHA1.Create();
            var hashArray = sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(sigStr));
            StringBuilder sb = new StringBuilder();
            foreach (var b in hashArray)
            {
                sb.Append(b.ToString("X2"));
            }

            rowInfo.CreateCell(colIdx).SetCellValue($"::{sb.ToString()}{sigStr}");


            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Close();
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"exported{DateTime.Now.ToString("yyyyMMdd.HHmmss")}.xlsx");
        }

    }
}