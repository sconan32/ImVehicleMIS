using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Core.Specifications;
using Socona.ImVehicle.Web.ViewModels;
using Socona.ImVehicle.Infrastructure.Specifications;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Socona.ImVehicle.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly UserManager<VehicleUser> _userManager;
        public GroupController(IGroupService groupService, UserManager<VehicleUser> userManager)
        {
            _groupService = groupService;
            _userManager = userManager;
        }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> GetDropDownList(long? townId)
        {
            var gs = await _groupService.ListGroupsForTownEagerAsync(HttpContext.User, townId ?? -1);

            var list = gs.Select(t => new { Value = t.Id, Text = t.Name });
            return new JsonResult(list);

        }

        [Authorize(Roles = "TownManager,Admins")]
        public async Task<IActionResult> LoadData(int? townId, int? page = 0, int? pageSize = 20)
        {
            ISpecification<GroupItem> canFetch = await Group4UserSpecification.CreateAsync(HttpContext.User, _userManager);

            if (townId != null)
            {
                var inGroup = new GroupsInTownSpecification(townId.Value);
                canFetch = canFetch.And(inGroup);
            }
            canFetch.Includes.Add(t => t.Drivers);
            canFetch.Includes.Add(t => t.SecurityPersons);
            canFetch.Includes.Add(t => t.Vehicles);
            canFetch.Includes.Add(t => t.UserFiles);
            canFetch.Includes.Add(t => t.Town);

            var startIdx = (page) * pageSize ?? 0;
            startIdx = Math.Max(0, startIdx);
            var groups = await _groupService.ListRangeAsync(canFetch, startIdx, pageSize ?? 0);
            var list = groups.Select(t => new GroupListViewModel(t));
            return new JsonResult(list);
        }

        [Authorize(Roles = "GlobalVisitor,TownManager,Admins")]
        public async Task<IActionResult> ExportExcel(long? townId, long? groupId)
        {
            ISpecification<GroupItem> canFetch = await Group4UserSpecification.CreateAsync(HttpContext.User, _userManager);


            if (townId != null)
            {
                var inTown = new Specification<GroupItem>(t => t.TownId == townId);
                canFetch = canFetch.And(inTown);
            }
            if (groupId != null)
            {
                var inGroup = new Specification<GroupItem>(t => t.Id == groupId);
                canFetch = canFetch.And(inGroup);
            }
            canFetch.Includes.Add(t => t.Vehicles);
            canFetch.Includes.Add(t => t.Town);
            canFetch.Includes.Add(t => t.Drivers);
            var items = await _groupService.ListAsync(canFetch);
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
            sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 12));


            rowIdx += 2;
            colIdx = 0;
            var rowSummary = sheet1.CreateRow(rowIdx);

            rowSummary.CreateCell(colIdx).SetCellValue($"共 {items.Count} 条记录 ");

            
            rowIdx += 1;
            colIdx = 0;
            var rowHeader = sheet1.CreateRow(rowIdx);

            rowHeader.CreateCell(colIdx++).SetCellValue("名称");
            rowHeader.CreateCell(colIdx++).SetCellValue("单位类型");
            rowHeader.CreateCell(colIdx++).SetCellValue("负责人");
            rowHeader.CreateCell(colIdx++).SetCellValue("联系电话");
            rowHeader.CreateCell(colIdx++).SetCellValue("监理中队");
            rowHeader.CreateCell(colIdx++).SetCellValue("监理民警");
            rowHeader.CreateCell(colIdx++).SetCellValue("状态");
            rowHeader.CreateCell(colIdx++).SetCellValue("司机数");
            rowHeader.CreateCell(colIdx++).SetCellValue("车辆数");
            rowHeader.CreateCell(colIdx++).SetCellValue("街道");

            rowHeader.Cells.ForEach(t => { t.CellStyle = styHeader; });
            sheet1.SetColumnWidth(0, 600 * 10);
            sheet1.SetColumnWidth(1, 200 * 10);
            sheet1.SetColumnWidth(2, 200 * 10);
            sheet1.SetColumnWidth(3, 310 * 10);
            sheet1.SetColumnWidth(4, 450 * 10);
            sheet1.SetColumnWidth(5, 200 * 10);
            sheet1.SetColumnWidth(6, 200 * 10);
            sheet1.SetColumnWidth(7, 130 * 10);
            sheet1.SetColumnWidth(8, 200 * 10);
            sheet1.SetColumnWidth(9, 330 * 10);

            for (int i = 0; i < items.Count; i++)
            {
                rowIdx += 1;
                colIdx = 0;
                IRow row = sheet1.CreateRow(rowIdx);
                row.CreateCell(colIdx++).SetCellValue(items[i].Name);
                row.CreateCell(colIdx++).SetCellValue(items[i].Type);
                row.CreateCell(colIdx++).SetCellValue(items[i].ChiefName);
                row.CreateCell(colIdx++).SetCellValue(items[i].ChiefTel);
                row.CreateCell(colIdx++).SetCellValue(items[i].PoliceOffice);
                row.CreateCell(colIdx++).SetCellValue(items[i].Policeman);
                row.CreateCell(colIdx++).SetCellValue(items[i].Drivers?.Count ?? 0);
                row.CreateCell(colIdx++).SetCellValue(items[i].Vehicles?.Count ?? 0);
                row.CreateCell(colIdx++).SetCellValue(items[i].Town?.Name);

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
