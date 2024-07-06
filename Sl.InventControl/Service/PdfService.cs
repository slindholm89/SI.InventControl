using System.Diagnostics;
using System.Reflection.Metadata;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MudBlazor;
using PdfSharp;
using PdfSharp.Charting;
using PdfSharp.Drawing;
using PdfSharp.Drawing.BarCodes;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Content.Objects;
using PdfSharp.Snippets.Font;
using Sl.InventControl.Data;
using static MudBlazor.CategoryTypes;

namespace Sl.InventControl.Service {
    public class PdfService {

        public PdfService(IConfiguration configuration) {

            ReportsFolder = new SettingsModel(configuration).Paths.WorkingFolder;

            // NET6FIX - will be removed
            if (Capabilities.Build.IsCoreBuild)
                GlobalFontSettings.FontResolver = new FailsafeFontResolver();
            


            _document = new PdfDocument();
            _document.Info.Title = _dateNow + "- US";
            _document.Info.Subject = "US";

        }


        private PdfDocument _document;
        private PdfPage _page;
        private XGraphics _gfx;

        public string ReportsFolder { get; set; } = $"C:\\";

        private string _dateNow => DateTime.Now.ToString("yyyy-MM-dd");



        public async Task<string> CreateUSWrapper(LoanModel loan) {
            _page = _document.AddPage();
            _gfx = XGraphics.FromPdfPage(_page);

            AddPageHeader();

            AddLoanInformation(loan);

            AddFooter(loan);

            return SaveReport(loan.Id);
        }


        private void AddPageHeader() {
            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
            XFont font = new XFont("Arial", 25, XFontStyleEx.Bold, options);
            _gfx.DrawString("US", font, XBrushes.Black, new XRect(0, 20, _page.Width, _page.Height), XStringFormats.TopCenter);

            var pen = new XPen(XColors.DarkBlue, 3);
            _gfx.DrawLine(pen, 100, 55, _page.Width - 100, 55);
        }


        private void AddLoanInformation(LoanModel loan) {
            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
            XFont hfont = new XFont("Arial", 12, XFontStyleEx.Bold, options);
            XFont font = new XFont("Arial", 12, XFontStyleEx.Regular, options);
            
            double ownerHeight = 80;
            _gfx.DrawString("Owner:", hfont, XBrushes.Black, new XRect(100, ownerHeight, _page.Width, _page.Height), XStringFormats.TopLeft);
            if (!string.IsNullOrEmpty(loan.OwnerEmployee.FirstName)) {
                ownerHeight += 15;
                _gfx.DrawString(loan.OwnerEmployee.FirstName + " " + loan.OwnerEmployee.LastName, font, XBrushes.Black, new XRect(100, ownerHeight, _page.Width, _page.Height), XStringFormats.TopLeft);
            }

            if (!string.IsNullOrEmpty(loan.OwnerEmployee.Department)) {
                ownerHeight += 15;
                _gfx.DrawString(loan.OwnerEmployee.Department, font, XBrushes.Black, new XRect(100, ownerHeight, _page.Width, _page.Height), XStringFormats.TopLeft);
            }

            if (!string.IsNullOrEmpty(loan.OwnerEmployee.PhoneNumber)) {
                ownerHeight += 15;
                _gfx.DrawString(loan.OwnerEmployee.PhoneNumber, font, XBrushes.Black, new XRect(100, ownerHeight, _page.Width, _page.Height), XStringFormats.TopLeft);
            }
            
            if (!string.IsNullOrEmpty(loan.OwnerEmployee.Email)) {
                ownerHeight += 15;
                _gfx.DrawString(loan.OwnerEmployee.Email, font, XBrushes.Black, new XRect(100, ownerHeight, _page.Width, _page.Height), XStringFormats.TopLeft);
            }


            double lenderHeight = 80;
            _gfx.DrawString("Lender:", hfont, XBrushes.Black, new XRect(_page.Width-200, lenderHeight, _page.Width, (double)(_page.Height)), XStringFormats.TopLeft);
            if (!string.IsNullOrEmpty(loan.BorowingEmployee.FirstName)) {
                lenderHeight += 15;
                _gfx.DrawString(loan.BorowingEmployee.FirstName + " " + loan.BorowingEmployee.LastName, font, XBrushes.Black, new XRect(_page.Width - 200, lenderHeight, _page.Width, _page.Height), XStringFormats.TopLeft);
            }

            if (!string.IsNullOrEmpty(loan.BorowingEmployee.Department)) {
                lenderHeight += 15;
                _gfx.DrawString(loan.BorowingEmployee.Department, font, XBrushes.Black, new XRect(_page.Width - 200, lenderHeight, _page.Width, _page.Height), XStringFormats.TopLeft);
            }

            if (!string.IsNullOrEmpty(loan.BorowingEmployee.PhoneNumber)) {
                lenderHeight += 15;
                _gfx.DrawString(loan.BorowingEmployee.PhoneNumber, font, XBrushes.Black, new XRect(_page.Width - 200, lenderHeight, _page.Width, _page.Height), XStringFormats.TopLeft);
            }

            if (!string.IsNullOrEmpty(loan.BorowingEmployee.Email)) {
                lenderHeight += 15;
                _gfx.DrawString(loan.BorowingEmployee.Email, font, XBrushes.Black, new XRect(_page.Width - 200, lenderHeight, _page.Width, _page.Height), XStringFormats.TopLeft);
            }

            ownerHeight += 25;
            _gfx.DrawString("US-number:", hfont, XBrushes.Black, new XRect(100, ownerHeight, _page.Width, (double)(_page.Height)), XStringFormats.TopLeft);
            _gfx.DrawString(loan.Id, hfont, XBrushes.Black, new XRect(200, ownerHeight, _page.Width, (double)(_page.Height)), XStringFormats.TopLeft);
            //Code2of5Interleaved bc39 = new Code2of5Interleaved(usId, new XSize(100, 50),CodeDirection.LeftToRight);
            //bc39.TextLocation = TextLocation.AboveEmbedded;
            //_gfx.DrawBarCode(bc39, XBrushes.DarkBlue, new XPoint(100, ownerHeight));

            ownerHeight += 25;
            _gfx.DrawString("Return items by:", hfont, XBrushes.Black, new XRect(100, ownerHeight, _page.Width, (double)(_page.Height)), XStringFormats.TopLeft);
            _gfx.DrawString(loan.ReturnByDate?.ToString("yyyy-MM-dd"), hfont, XBrushes.Black, new XRect(200, ownerHeight, _page.Width, (double)_page.Height), XStringFormats.TopLeft);

            ownerHeight += 25;
            _gfx.DrawString("Lended items:", hfont, XBrushes.Black, new XRect(100, ownerHeight, _page.Width, (double)(_page.Height)), XStringFormats.TopLeft);
            ownerHeight += 25;
            _gfx.DrawString("Type:", hfont, XBrushes.Black, new XRect(100, ownerHeight, _page.Width, (double)_page.Height), XStringFormats.TopLeft);
            _gfx.DrawString("Manufacturer:", hfont, XBrushes.Black, new XRect(200, ownerHeight, _page.Width, (double)_page.Height), XStringFormats.TopLeft);
            _gfx.DrawString("Model:", hfont, XBrushes.Black, new XRect(300, ownerHeight, _page.Width, (double)_page.Height), XStringFormats.TopLeft);
            _gfx.DrawString("Serial number:", hfont, XBrushes.Black, new XRect(400, ownerHeight, _page.Width, (double)_page.Height), XStringFormats.TopLeft);

            var pen = new XPen(XColors.Black, 2);
            _gfx.DrawLine(pen, 100, ownerHeight + 20, _page.Width - 100, ownerHeight + 20);
            foreach (var item in loan.Items) {
                ownerHeight += 25;
                _gfx.DrawString(item.Type.Type, font, XBrushes.Black, new XRect(100, ownerHeight, _page.Width, (double)_page.Height), XStringFormats.TopLeft);
                _gfx.DrawString(item.Type.Make, font, XBrushes.Black, new XRect(200, ownerHeight, _page.Width, (double)_page.Height), XStringFormats.TopLeft);
                _gfx.DrawString(item.Type.Model, font, XBrushes.Black, new XRect(300, ownerHeight, _page.Width, (double)_page.Height), XStringFormats.TopLeft);
                _gfx.DrawString(item.SerialNumber, font, XBrushes.Black, new XRect(400, ownerHeight, _page.Width, (double)_page.Height), XStringFormats.TopLeft);
            }
            _gfx.DrawLine(pen, 100, ownerHeight + 20, _page.Width - 100, ownerHeight + 20);

        }
        
        private void AddFooter(LoanModel loan) {

            var pen = new XPen(XColors.Black, 2);
            _gfx.DrawLine(pen, 100, _page.Height - 60, 250, _page.Height - 60); // Left sign lign
            _gfx.DrawLine(pen, _page.Width - 250, _page.Height - 60, _page.Width - 100, _page.Height - 60); // Right Sign lign

            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
            XFont font = new XFont("Arial", 15, XFontStyleEx.Regular, options);
            _gfx.DrawString(loan.BorowingEmployee.FirstName + " " + loan.BorowingEmployee.LastName, 
                font, XBrushes.Black, new XRect(100, 25, 100,_page.Height - 65), XStringFormats.BottomLeft);

            _gfx.DrawString(DateTime.Now.ToString("yyyy-MM-dd"),
                font, XBrushes.Black, new XRect(100, 25, 100, _page.Height - 50), XStringFormats.BottomLeft);


            _gfx.DrawString("Returned:",
                font, XBrushes.Black, new XRect(0, 25, _page.Width - 180, _page.Height - 125), XStringFormats.BottomRight);


        }

        private string SaveReport(string usNumber) {
            
            var filePath = Path.Combine(ReportsFolder, "US-"+usNumber+".pdf");
            _document.Save(filePath);
            return filePath;
        }

        public void OpenReport(string usNumber) {
            var filePath = Path.Combine(ReportsFolder, "US-" + usNumber + ".pdf");
            if (File.Exists(filePath)) {
                System.Diagnostics.Process.Start(filePath);
            } else {
                throw new FileNotFoundException(filePath);
            }
        }

    }
}
