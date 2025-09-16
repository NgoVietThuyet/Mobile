using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;

namespace Common
{
    /// <summary>
    /// Không chỉnh sửa các file common
    /// </summary>
    public static class ExportExtension
    {
        public static async Task<byte[]> ExportToExcel<T>(IEnumerable<T> data, string sheetName = "Sheet1", string? title = null, Dictionary<string, string>? additionalInfo = null)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            var stream = new MemoryStream();
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets.Add(sheetName);

            var properties = typeof(T).GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(DescriptionAttribute)))
                .ToList();

            worksheet.Cells["A3"].Value = $"BÁO CÁO TỔNG HỢP {title ?? string.Empty}";
            worksheet.Cells["A3"].Style.Font.Bold = true;

            // Tạo tiêu đề cho các cột
            int rowIn = 5;
            if (additionalInfo != null)
            {
                foreach (var info in additionalInfo)
                {
                    worksheet.Cells[rowIn, 1].Value = info.Key;
                    worksheet.Cells[rowIn, 2].Value = info.Value;
                    rowIn++;
                }
            }
            // Tạo tiêu đề cho các cột
            int colIndex = 1;
            var rowIndex = 7;
            CreateHeader(properties, worksheet, rowIndex, ref colIndex);

            using (var r = worksheet.Cells[1, 1, 1, colIndex - 1])
            {
                r.Merge = true;
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }
            using (var r = worksheet.Cells[3, 1, 3, colIndex - 1])
            {
                r.Merge = true;
                r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
            }
            using (var r = worksheet.Cells[4, 1, 4, colIndex - 1])
            {
                r.Merge = true;
                r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
            }
            // Ghi dữ liệu vào các dòng
            rowIndex++;
            foreach (var item in data)
            {
                colIndex = 1;
                ExportObjectToExcel(item, properties, worksheet, ref rowIndex, ref colIndex);
            }
            worksheet.Cells.AutoFitColumns();

            return await package.GetAsByteArrayAsync();
        }
        private static void CreateHeader(List<PropertyInfo> properties, ExcelWorksheet worksheet, int rowIndex, ref int colIndex)
        {
            for (var i = 0; i < properties.Count; i++)
            {
                var description = ((DescriptionAttribute)properties[i]
                    .GetCustomAttributes(typeof(DescriptionAttribute), true)
                    .FirstOrDefault())?.Description;
                if (description != "ListObject")
                {
                    worksheet.Cells[rowIndex, colIndex].Value = description ?? properties[i].Name;
                    worksheet.Cells[rowIndex, colIndex].Style.Font.Size = 12;
                    worksheet.Cells[rowIndex, colIndex].Style.Font.Bold = true;
                    worksheet.Cells[rowIndex, colIndex].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowIndex, colIndex].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[rowIndex, colIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[rowIndex, colIndex].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    colIndex++;
                }
                else
                {
                    var genericType = properties[i].PropertyType.GetGenericArguments()[0];
                    var prps = genericType.GetProperties().ToList();
                    CreateHeader(prps, worksheet, rowIndex, ref colIndex);
                }
            }
        }

        private static void ExportObjectToExcel(object obj, List<PropertyInfo> properties, ExcelWorksheet worksheet, ref int rowIndex, ref int colIndex)
        {
            foreach (var property in properties)
            {
                var value = property.GetValue(obj);

                if (value is IEnumerable<object> collection)
                {
                    foreach (var item in collection.Where(x => x != null))
                    {
                        var prps = value.GetType().GetGenericArguments()[0].GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(DescriptionAttribute)))
                                     .ToList();
                        colIndex = GetColumnIndex(property);
                        ExportObjectToExcel(item, prps, worksheet, ref rowIndex, ref colIndex);
                    }
                    rowIndex--;
                }
                else
                {
                    worksheet.Cells[rowIndex, colIndex].Value = value;
                    colIndex++;
                }
            }
            rowIndex++;
        }

        private static int GetColumnIndex(PropertyInfo property)
        {
            var properties = property.DeclaringType.GetProperties()
                  .Where(prop => Attribute.IsDefined(prop, typeof(DescriptionAttribute)))
                  .ToList();

            return properties.IndexOf(property) + 1;
        }
    }
}
