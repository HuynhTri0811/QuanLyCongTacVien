using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ClosedXML.Excel;
using Microsoft.Win32;

namespace QuanLyCongTacVien.Helpers
{
    public static class ExcelExportHelper
    {
        public static void ExportToExcel<T>(IEnumerable<T> data, string fileName, string sheetName = "Sheet1")
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = fileName
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add(sheetName);
                    var properties = typeof(T).GetProperties()
                        .Where(p => p.GetCustomAttribute<System.ComponentModel.BrowsableAttribute>()?.Browsable != false)
                        .ToList();

                    // Headers
                    for (int i = 0; i < properties.Count; i++)
                    {
                        var displayName = properties[i].GetCustomAttribute<System.ComponentModel.DisplayNameAttribute>()?.DisplayName 
                                          ?? properties[i].Name;
                        worksheet.Cell(1, i + 1).Value = displayName;
                        worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                        worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                    }

                    // Data
                    var list = data.ToList();
                    for (int row = 0; row < list.Count; row++)
                    {
                        for (int col = 0; col < properties.Count; col++)
                        {
                            var value = properties[col].GetValue(list[row]);
                            if (value != null)
                            {
                                if (value is DateTime dt)
                                    worksheet.Cell(row + 2, col + 1).Value = dt.ToString("dd/MM/yyyy");
                                else
                                    worksheet.Cell(row + 2, col + 1).Value = value.ToString();
                            }
                        }
                    }

                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs(saveFileDialog.FileName);
                }

                System.Windows.MessageBox.Show("Xuất Excel thành công!", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
        }
    }
}
