using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Extentions
{
    public static class ExcelNPOIExtention
    {
        public static void SetCellValueText(IRow row, int columnIndex, object value, ICellStyle style)
        {
            var cell = row.CreateCell(columnIndex);
            cell.CellStyle = style;

            if (value is null)
            {
                cell.SetCellValue("");
            }
            else
            {
                cell.SetCellValue(value.ToString());
            }
        }
        public static void SetCellValueNumber(IRow row, int columnIndex, object value, ICellStyle style)
        {
            var cell = row.CreateCell(columnIndex);
            cell.CellStyle = style;

            if (value is null)
            {
                cell.SetCellValue("");
            }
            else if (double.TryParse(value.ToString(), out double number))
            {
                if (number == 0)
                {
                    cell.SetCellValue("");
                }
                else
                {
                    cell.SetCellValue(number);
                }
            }
            else
            {
                cell.SetCellValue("");
            }
        }
        public static void SetCellValue(IRow row, int columnIndex, object value, ICellStyle style)
        {
            var cell = row.CreateCell(columnIndex);
            cell.CellStyle = style;

            if (value is null)
            {
                cell.SetCellValue("");
            }
            else if (double.TryParse(value.ToString(), out double number))
            {
                if (number == 0)
                {
                    cell.SetCellValue("");
                }
                else
                {
                    cell.SetCellValue(number);
                }
            }
            else
            {
                cell.SetCellValue(value.ToString());
            }
        }
        public static ICellStyle SetCellStyleText(IWorkbook workbook, bool isBold, HorizontalAlignment align, bool isBorder)
        {
            ICellStyle style = workbook.CreateCellStyle();
            IFont font = workbook.CreateFont();
            font.FontName = "Times New Roman";
            font.FontHeightInPoints = 12;
            style.SetFont(font);
            font.IsBold = isBold;
            style.Alignment = align;
            if (isBorder)
            {
                style.BorderTop = BorderStyle.Thin;
                style.BorderBottom = BorderStyle.Thin;
                style.BorderLeft = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
            }
            style.DataFormat = workbook.CreateDataFormat().GetFormat("@");
            return style;
        }
        public static ICellStyle SetCellStyleNumber(IWorkbook workbook, bool isBold, HorizontalAlignment align, bool isBorder)
        {
            ICellStyle style = workbook.CreateCellStyle();
            IFont font = workbook.CreateFont();
            font.FontName = "Times New Roman";
            font.FontHeightInPoints = 12;
            font.IsBold = isBold;
            style.SetFont(font);
            style.Alignment = align;
            if (isBorder)
            {
                style.BorderTop = BorderStyle.Thin;
                style.BorderBottom = BorderStyle.Thin;
                style.BorderLeft = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
            }
            style.DataFormat = workbook.CreateDataFormat().GetFormat("#,##0");
            return style;
        }
    }
}
