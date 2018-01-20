using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Socona.ImVehicle.Web.ViewModels;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Socona.ImVehicle.Infrastructure.Extensions;

namespace Socona.ImVehicle.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class VehicleController : Controller
    {

        private readonly IVehicleService _vehicleService;
        private readonly UserManager<VehicleUser> _userManager;
        public VehicleController(IVehicleService vehicleService, UserManager<VehicleUser> userManager)
        {
            _vehicleService = vehicleService;
            _userManager = userManager;
        }



        public async Task<IActionResult> LoadData(int? townId, int? page = 0, int? pageSize = 20)
        {
            ISpecification<VehicleItem> canFetch = await Vehicle4UserSpecification.CreateAsync(HttpContext.User, _userManager);
            if (townId != null)
            {
                var inTown = new VehicleInTownSpecification(townId.Value);
                canFetch = canFetch.And(inTown);
            }


            canFetch.Includes.Add(t => t.Driver);
            canFetch.Includes.Add(t => t.Town);
            canFetch.Includes.Add(t => t.Group);

            var startIdx = (page) * pageSize ?? 0;
            startIdx = Math.Max(0, startIdx);
            var groups = await _vehicleService.ListRangeAsync(canFetch, startIdx, pageSize ?? 0);
            var list = groups.Select(t => new VehicleListViewModel(t));
            return new JsonResult(list);
        }

        [Authorize(Roles = "GlobalVisitor,TownManager,Admins")]
        public async Task<IActionResult> ExportExcel(long? townId, long? groupId)
        {
            ISpecification<VehicleItem> canFetch = await Vehicle4UserSpecification.CreateAsync(HttpContext.User, _userManager);


            if (townId != null)
            {
                var inTown = new VehicleInTownSpecification(townId.Value);
                canFetch = canFetch.And(inTown);
            }
            if (groupId != null)
            {
                var inGroup = new Specification<VehicleItem>(t => t.GroupId == groupId);
                canFetch = canFetch.And(inGroup);
            }
         
            canFetch.Includes.Add(t => t.Town);
            canFetch.Includes.Add(t => t.Group);
            canFetch.Includes.Add(t => t.Driver);
            var items = await _vehicleService.ListAsync(canFetch);
            items = items.OrderBy(t => t.IsValid()).ThenBy(t => t.YearlyAuditDate).ToList();
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



            sheet1.CreateRow(rowIdx).CreateCell(colIdx).SetCellValue($"车辆列表");
            sheet1.GetRow(rowIdx).GetCell(0).CellStyle = styTitle;
            sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 13));


            rowIdx += 2;
            colIdx = 0;
            var rowSummary = sheet1.CreateRow(rowIdx);

            rowSummary.CreateCell(colIdx).SetCellValue($"共 {items.Count} 条记录 ");


            rowIdx += 1;
            colIdx = 0;
            var rowHeader = sheet1.CreateRow(rowIdx);
            rowHeader.Height = 70 * 10;
            rowHeader.CreateCell(colIdx++).SetCellValue("编号");
            rowHeader.CreateCell(colIdx++).SetCellValue("车牌号");
            rowHeader.CreateCell(colIdx++).SetCellValue("类型");
            rowHeader.CreateCell(colIdx++).SetCellValue("用途");
            rowHeader.CreateCell(colIdx++).SetCellValue("GPS");
            rowHeader.CreateCell(colIdx++).SetCellValue("年检有效至");
            rowHeader.CreateCell(colIdx++).SetCellValue("报废日期");
            rowHeader.CreateCell(colIdx++).SetCellValue("实际车主");
            rowHeader.CreateCell(colIdx++).SetCellValue("挂靠单位");
            rowHeader.CreateCell(colIdx++).SetCellValue("状态");
            rowHeader.CreateCell(colIdx++).SetCellValue("驾驶员");
            rowHeader.CreateCell(colIdx++).SetCellValue("电话");
            rowHeader.CreateCell(colIdx++).SetCellValue("街道");
            rowHeader.CreateCell(colIdx++).SetCellValue("安全组");

            sheet1.CreateFreezePane(0, 4);

            rowHeader.Cells.ForEach(t => { t.CellStyle = styHeader; });



            sheet1.SetColumnWidth(0, 120 * 10);
            sheet1.SetColumnWidth(1, 230 * 10);
            sheet1.SetColumnWidth(2, 250 * 10);
            sheet1.SetColumnWidth(3, 220 * 10);
            sheet1.SetColumnWidth(4, 90 * 10);
            sheet1.SetColumnWidth(5, 260 * 10);
            sheet1.SetColumnWidth(6, 260 * 10);
            sheet1.SetColumnWidth(7, 220 * 10);
            sheet1.SetColumnWidth(8, 270 * 10);
            sheet1.SetColumnWidth(9, 130 * 10);
            sheet1.SetColumnWidth(10, 200 * 10);
            sheet1.SetColumnWidth(11, 310 * 10);
            sheet1.SetColumnWidth(12, 330 * 10);
            sheet1.SetColumnWidth(13, 600 * 10);
           


            for (int i = 0; i < items.Count; i++)
            {
                rowIdx += 1;
                colIdx = 0;
                IRow row = sheet1.CreateRow(rowIdx);
                row.Height = 32 * 10;
                row.CreateCell(colIdx++).SetCellValue(i + 1);
                row.CreateCell(colIdx++).SetCellValue(items[i].LicenceNumber);
                row.CreateCell(colIdx++).SetCellValue(items[i].Type.GetDisplayName());
                row.CreateCell(colIdx++).SetCellValue(items[i].Usage.GetDisplayName());
                row.CreateCell(colIdx++).SetCellValue(items[i].GpsEnabled==true?"是":"否");
                row.CreateCell(colIdx++).SetCellValue(items[i].YearlyAuditDate?.ToShortDateString());
                row.CreateCell(colIdx++).SetCellValue(items[i].DumpDate?.ToShortDateString());
                row.CreateCell(colIdx++).SetCellValue(items[i].RealOwner);
                row.CreateCell(colIdx++).SetCellValue(items[i].Agent);
                row.CreateCell(colIdx++).SetCellValue(items[i].IsValid() ? "正常" : "预警");
                row.CreateCell(colIdx++).SetCellValue(items[i].Driver?.Name);
                row.CreateCell(colIdx++).SetCellValue(items[i].Driver?.Tel);
                row.CreateCell(colIdx++).SetCellValue(items[i].Town?.Name);
                row.CreateCell(colIdx++).SetCellValue(items[i].Group?.Name);
                row.Cells.ForEach(t => { t.CellStyle = styContent; });
            }


            rowIdx += 1;
            colIdx = 0;
            var rowInfo = sheet1.CreateRow(rowIdx);
            var sigStr = $"::{HttpContext.User.Identity.Name}::socona.imvehicle.vehicle.export?town={townId}&group={groupId}::{DateTime.Now.ToString("yyyyMMdd.HHmmss")}";
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