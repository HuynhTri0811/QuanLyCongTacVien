using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace QuanLyCongTacVien.Helpers
{
    public static class WordExportHelper
    {
        public static void ExportContractToWord(Models.ChiTietHopDong item, Models.QuanLyHopDongCongTacVien? parentInfo, Models.CongTacVien? ctv)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Documents (*.docx)|*.docx",
                FileName = $"HopDong_{item.SoHopDong ?? "Moi"}_{ctv?.HoVaTen ?? "CTV"}.docx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    string filePath = saveFileDialog.FileName;
                    CreateWordDocument(filePath, item, parentInfo, ctv);
                    CustomMessageBox.Show("Xuất file Word thành công!", "Thông báo", MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    CustomMessageBox.Show($"Lỗi xuất file Word: {ex.Message}\n\nChi tiết: {ex.StackTrace}", "Lỗi", MessageBoxImage.Error);
                }
            }
        }

        private static void CreateWordDocument(string filePath, Models.ChiTietHopDong item, Models.QuanLyHopDongCongTacVien? parentInfo, Models.CongTacVien? ctv)
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                // Add a main document part.
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                // Create the document structure
                mainPart.Document = new Document();
                Body body = new Body();
                mainPart.Document.Append(body);

                // Page setup (margins: 2cm top/bottom, 2.5cm left/right)
                SectionProperties sectionProps = new SectionProperties();
                PageMargin pageMargin = new PageMargin() 
                { 
                    Top = 1134, // 2 cm (1 inch = 1440 dxa)
                    Bottom = 1134, 
                    Left = 1440, // 2.54 cm
                    Right = 1440 
                };
                sectionProps.Append(pageMargin);
                body.Append(sectionProps);

                // Add Emblem Header
                AddEmblemHeader(body, parentInfo?.TruongCongTy);

                // Title
                AddParagraph(body, "THỎA THUẬN HỢP ĐỒNG LAO ĐỘNG", 16, true, JustificationValues.Center, 240, 120);
                AddParagraph(body, $"Số: {item.SoHopDong ?? "........................"}", 12, false, JustificationValues.Center, 0, 240, true);

                // Preamble
                AddParagraph(body, "- Căn cứ Bộ luật Lao động nước Cộng hòa xã hội chủ nghĩa Việt Nam;", 11, false, JustificationValues.Left, 0, 60, true);
                AddParagraph(body, "- Căn cứ nhu cầu và khả năng của hai bên;", 11, false, JustificationValues.Left, 0, 120, true);

                string schoolName = parentInfo?.TruongCongTy ?? "..................................................";
                string dateStr = item.NgayKy?.ToString("dd/MM/yyyy") ?? "..../..../........";
                AddParagraph(body, $"Hôm nay, ngày {dateStr}, tại {schoolName}, chúng tôi gồm các bên dưới đây:", 12, false, JustificationValues.Left, 120, 240);

                // Party A
                AddParagraph(body, "BÊN A: NGƯỜI SỬ DỤNG LAO ĐỘNG", 12, true, JustificationValues.Left, 120, 60);
                AddParagraph(body, $"- Đại diện đơn vị: {schoolName}", 12, false, JustificationValues.Left, 0, 60);
                AddParagraph(body, $"- Bộ phận làm việc: {item.BoPhan ?? ".................................................."}", 12, false, JustificationValues.Left, 0, 120);

                // Party B
                AddParagraph(body, "BÊN B: NGƯỜI LAO ĐỘNG (CỘNG TÁC VIÊN)", 12, true, JustificationValues.Left, 120, 60);
                string ctvName = ctv?.HoVaTen ?? "..................................................";
                string ctvId = item.MaNhanSu ?? ctv?.MaNhanSu ?? "........................";
                AddParagraph(body, $"- Họ và tên: {ctvName}", 12, false, JustificationValues.Left, 0, 60);
                AddParagraph(body, $"- Mã nhân sự: {ctvId}", 12, false, JustificationValues.Left, 0, 60);

                string cmnd = ctv?.SoCMND ?? "........................";
                string ngayCap = ctv?.NgayCap != null && ctv.NgayCap != DateTime.MinValue ? ctv.NgayCap.ToString("dd/MM/yyyy") : "..../..../........";
                string noiCap = ctv?.NoiCap ?? "..................................................";
                AddParagraph(body, $"- Số CMND/CCCD: {cmnd}    - Ngày cấp: {ngayCap}    - Nơi cấp: {noiCap}", 12, false, JustificationValues.Left, 0, 60);

                string phone = ctv?.DienThoaiDiDong ?? "........................";
                AddParagraph(body, $"- Điện thoại di động: {phone}", 12, false, JustificationValues.Left, 0, 60);

                string thuongTru = ctv?.DiaChiThuongTru ?? "..................................................";
                AddParagraph(body, $"- Địa chỉ thường trú: {thuongTru}", 12, false, JustificationValues.Left, 0, 60);

                string bankAcc = ctv?.SoTaiKhoan ?? "........................";
                string bankName = ctv?.TenNganHang ?? "..................................................";
                AddParagraph(body, $"- Tài khoản ngân hàng: {bankAcc} tại Ngân hàng: {bankName}", 12, false, JustificationValues.Left, 0, 60);

                string degree = ctv?.HocVi ?? "";
                string major = ctv?.ChuyenNganhDaoTao ?? "";
                string degreeInfo = string.IsNullOrEmpty(degree) && string.IsNullOrEmpty(major) ? ".................................................." : $"{degree} - {major}";
                AddParagraph(body, $"- Trình độ chuyên môn: {degreeInfo}", 12, false, JustificationValues.Left, 0, 240);

                // Terms
                AddParagraph(body, "HAI BÊN CÙNG THỎA THUẬN VÀ THỐNG NHẤT CÁC ĐIỀU KHOẢN SAU:", 12, true, JustificationValues.Left, 120, 120);

                AddParagraph(body, "Điều 1: Thời hạn và công việc hợp đồng", 12, true, JustificationValues.Left, 60, 60);
                AddParagraph(body, $"- Loại hợp đồng: Hợp đồng lao động cộng tác viên", 12, false, JustificationValues.Left, 0, 60);
                AddParagraph(body, $"- Chức danh chuyên môn: {item.ChucDanh ?? ".................................................."}", 12, false, JustificationValues.Left, 0, 60);

                string tuNgay = item.TuNgay?.ToString("dd/MM/yyyy") ?? "..../..../........";
                string denNgay = item.DenNgay?.ToString("dd/MM/yyyy") ?? "..../..../........";
                AddParagraph(body, $"- Thời hạn hợp đồng: Từ ngày {tuNgay} đến ngày {denNgay}", 12, false, JustificationValues.Left, 0, 120);

                AddParagraph(body, "Điều 2: Hiệu lực và thỏa thuận khác", 12, true, JustificationValues.Left, 60, 60);
                string status = item.HetHieuLuc ? "Hết hiệu lực" : "Còn hiệu lực";
                AddParagraph(body, $"- Tình trạng hợp đồng: {status}", 12, false, JustificationValues.Left, 0, 60);
                string inLai = item.InLaiThoaThuan ? "Có" : "Không";
                AddParagraph(body, $"- Yêu cầu in lại thỏa thuận: {inLai}", 12, false, JustificationValues.Left, 0, 360);

                // Signatures
                AddSignaturesBlock(body);
            }
        }

        private static void AddEmblemHeader(Body body, string? schoolName)
        {
            Table table = new Table();
            
            TableProperties tblProp = new TableProperties(
                new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct },
                new TableBorders(
                    new TopBorder() { Val = BorderValues.None },
                    new BottomBorder() { Val = BorderValues.None },
                    new LeftBorder() { Val = BorderValues.None },
                    new RightBorder() { Val = BorderValues.None },
                    new InsideHorizontalBorder() { Val = BorderValues.None },
                    new InsideVerticalBorder() { Val = BorderValues.None }
                )
            );
            table.AppendChild(tblProp);

            TableRow row = new TableRow();
            
            // Column 1: School/Company name
            TableCell cell1 = new TableCell(new TableCellProperties(new TableCellWidth() { Width = "40%", Type = TableWidthUnitValues.Pct }));
            Paragraph p1 = new Paragraph(
                new ParagraphProperties(new Justification() { Val = JustificationValues.Center }),
                new Run(
                    new RunProperties(new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" }, new FontSize() { Val = "22" }, new Bold()),
                    new Text(string.IsNullOrEmpty(schoolName) ? "ĐƠN VỊ SỬ DỤNG" : schoolName.ToUpper())
                )
            );
            p1.Append(
                new Break(),
                new Run(
                    new RunProperties(new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" }, new FontSize() { Val = "20" }, new Italic()),
                    new Text("Bộ phận Nhân sự")
                )
            );
            cell1.Append(p1);
            row.Append(cell1);

            // Column 2: National emblem text
            TableCell cell2 = new TableCell(new TableCellProperties(new TableCellWidth() { Width = "60%", Type = TableWidthUnitValues.Pct }));
            Paragraph p2 = new Paragraph(
                new ParagraphProperties(new Justification() { Val = JustificationValues.Center }),
                new Run(
                    new RunProperties(new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" }, new FontSize() { Val = "22" }, new Bold()),
                    new Text("CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM")
                )
            );
            p2.Append(
                new Break(),
                new Run(
                    new RunProperties(new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" }, new FontSize() { Val = "22" }, new Bold()),
                    new Text("Độc lập - Tự do - Hạnh phúc")
                ),
                new Break(),
                new Run(
                    new RunProperties(new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" }, new FontSize() { Val = "20" }),
                    new Text("---------------")
                )
            );
            cell2.Append(p2);
            row.Append(cell2);

            table.Append(row);
            body.Append(table);

            // Empty spacing paragraph
            AddParagraph(body, "", 12, false, JustificationValues.Center, 0, 240);
        }

        private static void AddSignaturesBlock(Body body)
        {
            Table table = new Table();
            TableProperties tblProp = new TableProperties(
                new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct },
                new TableBorders(
                    new TopBorder() { Val = BorderValues.None },
                    new BottomBorder() { Val = BorderValues.None },
                    new LeftBorder() { Val = BorderValues.None },
                    new RightBorder() { Val = BorderValues.None },
                    new InsideHorizontalBorder() { Val = BorderValues.None },
                    new InsideVerticalBorder() { Val = BorderValues.None }
                )
            );
            table.AppendChild(tblProp);

            TableRow row = new TableRow();

            // Column A
            TableCell cellA = new TableCell(new TableCellProperties(new TableCellWidth() { Width = "50%", Type = TableWidthUnitValues.Pct }));
            Paragraph pA = new Paragraph(
                new ParagraphProperties(new Justification() { Val = JustificationValues.Center }),
                new Run(
                    new RunProperties(new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" }, new FontSize() { Val = "24" }, new Bold()),
                    new Text("ĐẠI DIỆN BÊN A")
                ),
                new Break(),
                new Run(
                    new RunProperties(new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" }, new FontSize() { Val = "20" }, new Italic()),
                    new Text("(Ký, ghi rõ họ tên)")
                )
            );
            cellA.Append(pA);
            row.Append(cellA);

            // Column B
            TableCell cellB = new TableCell(new TableCellProperties(new TableCellWidth() { Width = "50%", Type = TableWidthUnitValues.Pct }));
            Paragraph pB = new Paragraph(
                new ParagraphProperties(new Justification() { Val = JustificationValues.Center }),
                new Run(
                    new RunProperties(new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" }, new FontSize() { Val = "24" }, new Bold()),
                    new Text("NGƯỜI LAO ĐỘNG (BÊN B)")
                ),
                new Break(),
                new Run(
                    new RunProperties(new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" }, new FontSize() { Val = "20" }, new Italic()),
                    new Text("(Ký, ghi rõ họ tên)")
                )
            );
            cellB.Append(pB);
            row.Append(cellB);

            table.Append(row);
            body.Append(table);
        }

        private static void AddParagraph(Body body, string text, int fontSize, bool isBold, JustificationValues alignment, int spaceBefore, int spaceAfter, bool isItalic = false)
        {
            Paragraph p = new Paragraph();

            ParagraphProperties pProps = new ParagraphProperties();
            pProps.Append(new Justification() { Val = alignment });

            SpacingBetweenLines spacing = new SpacingBetweenLines() { Before = spaceBefore.ToString(), After = spaceAfter.ToString(), Line = "276", LineRule = LineSpacingRuleValues.Auto };
            pProps.Append(spacing);
            
            p.Append(pProps);

            Run run = new Run();
            RunProperties rProps = new RunProperties();
            rProps.Append(new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" });
            rProps.Append(new FontSize() { Val = (fontSize * 2).ToString() }); // OpenXml size is half-point

            if (isBold)
                rProps.Append(new Bold());
            if (isItalic)
                rProps.Append(new Italic());

            run.Append(rProps);
            run.Append(new Text(text));
            p.Append(run);

            body.Append(p);
        }
    }
}
