using DocumentFormat.OpenXml.Spreadsheet;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMO
{
    public class ReportUtilities
    {
        /// <summary>
        /// Tạo mới một dòng, và cell của nó
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="numRow"></param>
        public static IRow CreateRow(ref ISheet worksheet, int numRow, int numCell = 500)
        {
            var row = worksheet.GetRow(numRow);
            if (row == null)
            {
                worksheet.CreateRow(numRow);
                row = worksheet.GetRow(numRow);
            }
            for (int i = 0; i < numCell; i++)
            {
                CreateCell(ref row, i);
            }
            return row;
        }

        /// <summary>
        /// Tạo mới một dòng, và cell của nó
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="numRow"></param>
        public static IRow InsertRow(ref ISheet worksheet, int numRow, int numCell = 500)
        {
            var row = worksheet.GetRow(numRow);
            if (row == null)
            {
                worksheet.CreateRow(numRow);
                row = worksheet.GetRow(numRow);
            }
            worksheet.ShiftRows(numRow, numRow + 1, 1);
            row = worksheet.GetRow(numRow + 1);
            for (int i = 0; i < numCell; i++)
            {
                CreateCell(ref row, i);
            }
            return row;
        }

        /// <summary>
        /// Kiểm tra cell đã tồn tại chưa, nếu chưa thì create cell
        /// </summary>
        /// <param name="row"></param>
        /// <param name="numCell"></param>
        public static void CreateCell(ref IRow row, int numCell)
        {
            ICell cell = row.GetCell(numCell);

            if (cell == null)
            {
                row.CreateCell(numCell);
            }
        }

        public static void CopyRow(ref ISheet worksheet, int sourceRowNum, int destinationRowNum)
        {
            // Get the source / new row
            IRow newRow = worksheet.GetRow(destinationRowNum);
            IRow sourceRow = worksheet.GetRow(sourceRowNum);

            // If the row exist in destination, push down all rows by 1 else create a new row
            if (newRow != null)
            {
                worksheet.ShiftRows(destinationRowNum, worksheet.LastRowNum, 1, true, true);
            }
            else
            {
                newRow = worksheet.CreateRow(destinationRowNum);
            }

            //newRow.Height = sourceRow.Height;

            // Loop through source columns to add to new row
            for (int i = 0; i < sourceRow.LastCellNum; i++)
            {
                // Grab a copy of the old/new cell
                ICell oldCell = sourceRow.GetCell(i);
                ICell newCell = newRow.CreateCell(i);

                // If the old cell is null jump to next cell
                if (oldCell == null)
                {
                    newCell = null;
                    continue;
                }

                // Use old cell style
                newCell.CellStyle = oldCell.CellStyle;


                // Set the cell data value
                switch (oldCell.CellType)
                {
                    case NPOI.SS.UserModel.CellType.Blank:
                        break;
                    case NPOI.SS.UserModel.CellType.Boolean:
                        newCell.SetCellValue(oldCell.BooleanCellValue);
                        break;
                    case NPOI.SS.UserModel.CellType.Error:
                        newCell.SetCellValue(oldCell.ErrorCellValue);
                        break;
                    case NPOI.SS.UserModel.CellType.Formula:
                        newCell.SetCellValue(oldCell.CellFormula);
                        break;
                    case NPOI.SS.UserModel.CellType.Numeric:
                        newCell.SetCellValue(oldCell.NumericCellValue);
                        break;
                    case NPOI.SS.UserModel.CellType.String:
                        newCell.SetCellValue(oldCell.RichStringCellValue);
                        break;
                }
            }
        }

        public static void DeleteRow(ref ISheet sheet, IRow row)
        {
            sheet.RemoveRow(row);   // this only deletes all the cell values

            int rowIndex = row.RowNum;

            int lastRowNum = sheet.LastRowNum;

            if (rowIndex >= 0 && rowIndex < lastRowNum)
            {
                sheet.ShiftRows(rowIndex + 1, lastRowNum, -1);
            }
        }
    }
}