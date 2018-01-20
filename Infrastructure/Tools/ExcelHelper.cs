
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using Socona.ImVehicle.Core.Data;
using Socona.ImVehicle.Infrastructure.Extensions;

namespace Socona.ImVehicle.Infrastructure.Tools
{
public  static  class ExcelHelper
    {

        public static MemoryStream ExportGroups(List<GroupItem> items,string signature)
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            ISheet sheet1 = workbook.CreateSheet($"安全组列表");
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



            sheet1.CreateRow(rowIdx).CreateCell(colIdx).SetCellValue($"安全组列表");
            sheet1.GetRow(rowIdx).GetCell(0).CellStyle = styTitle;
            sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 10));


            rowIdx += 2;
            colIdx = 0;
            var rowSummary = sheet1.CreateRow(rowIdx);

            rowSummary.CreateCell(colIdx).SetCellValue($"共 {items.Count} 条记录 ");


            rowIdx += 1;
            colIdx = 0;
            var rowHeader = sheet1.CreateRow(rowIdx);

            rowHeader.CreateCell(colIdx++).SetCellValue("编号");
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

            sheet1.SetColumnWidth(0, 110 * 10);
            sheet1.SetColumnWidth(1, 600 * 10);
            sheet1.SetColumnWidth(2, 200 * 10);
            sheet1.SetColumnWidth(3, 200 * 10);
            sheet1.SetColumnWidth(4, 310 * 10);
            sheet1.SetColumnWidth(5, 420 * 10);
            sheet1.SetColumnWidth(6, 200 * 10);
            sheet1.SetColumnWidth(7, 130 * 10);
            sheet1.SetColumnWidth(8, 130 * 10);
            sheet1.SetColumnWidth(9, 130 * 10);
            sheet1.SetColumnWidth(10, 330 * 10);

            for (int i = 0; i < items.Count; i++)
            {
                rowIdx += 1;
                colIdx = 0;
                IRow row = sheet1.CreateRow(rowIdx);
                row.CreateCell(colIdx++).SetCellValue(i + 1);
                row.CreateCell(colIdx++).SetCellValue(items[i].Name);
                row.CreateCell(colIdx++).SetCellValue(items[i].Type);
                row.CreateCell(colIdx++).SetCellValue(items[i].ChiefName);
                row.CreateCell(colIdx++).SetCellValue(items[i].ChiefTel);
                row.CreateCell(colIdx++).SetCellValue(items[i].PoliceOffice);
                row.CreateCell(colIdx++).SetCellValue(items[i].Policeman);
                row.CreateCell(colIdx++).SetCellValue(items[i].IsValid() ? "正常" : "预警");
                row.CreateCell(colIdx++).SetCellValue(items[i].Drivers?.Count ?? 0);
                row.CreateCell(colIdx++).SetCellValue(items[i].Vehicles?.Count ?? 0);
                row.CreateCell(colIdx++).SetCellValue(items[i].Town?.Name);

                row.Cells.ForEach(t => { t.CellStyle = styContent; });
            }



            rowIdx += 1;
            colIdx = 0;

            var rowInfo = sheet1.CreateRow(rowIdx);
           
            var sha1 = SHA1.Create();
            var hashArray = sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(signature));
            StringBuilder sb = new StringBuilder();
            foreach (var b in hashArray)
            {
                sb.Append(b.ToString("X2"));
            }

            rowInfo.CreateCell(colIdx).SetCellValue($"::{sb.ToString()}{signature}");
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Close();
            return ms;
        }

        public static MemoryStream ExportDrivers(List<DriverItem> items, string signature)
        {
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



            sheet1.SetColumnWidth(0, 110 * 10);
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
           
            var sha1 = SHA1.Create();
            var hashArray = sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(signature));
            StringBuilder sb = new StringBuilder();
            foreach (var b in hashArray)
            {
                sb.Append(b.ToString("X2"));
            }

            rowInfo.CreateCell(colIdx).SetCellValue($"::{sb.ToString()}{signature}");


            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Close();
            return ms;
        }
        public static MemoryStream ExportVehicles(List<VehicleItem> items, string signature)
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            ISheet sheet1 = workbook.CreateSheet($"车辆列表");
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



            sheet1.SetColumnWidth(0, 110 * 10);
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
                row.CreateCell(colIdx++).SetCellValue(items[i].GpsEnabled == true ? "是" : "否");
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
            
            var sha1 = SHA1.Create();
            var hashArray = sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(signature));
            StringBuilder sb = new StringBuilder();
            foreach (var b in hashArray)
            {
                sb.Append(b.ToString("X2"));
            }

            rowInfo.CreateCell(colIdx).SetCellValue($"::{sb.ToString()}{signature}");


            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Close();
            return ms;
        }
    }
       
    
}