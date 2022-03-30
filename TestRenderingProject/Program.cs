using SelectPdf;
using System.Drawing;

class Program
{
    static void Main(string[] args)
    {
        SetupPDF();

        Console.WriteLine("Operation Finished. Press any key to exit.");
        Console.ReadKey();
    }

    private static void SetupPDF()
    {
        var input = AppDomain.CurrentDomain.BaseDirectory + "inputpdf.html";
        var request = File.ReadAllText(input);
        var bytes = RenderPdf(request);

        File.WriteAllBytes("Examples.pdf", bytes);
    }

    private static byte[] RenderPdf(string pdfHtml)
    {
        HtmlToPdf converter = new HtmlToPdf();
        var margin = 40;

        converter.Options.RenderingEngine = RenderingEngine.Blink;
        converter.Options.MarginBottom = margin;
        converter.Options.MarginTop = margin;
        converter.Options.MarginLeft = margin;
        converter.Options.MarginRight = margin;
        converter.Options.PageBreaksEnhancedAlgorithm = true;
        converter.Options.KeepTextsTogether = true;
        converter.Options.KeepImagesTogether = true;
        converter.Options.EmbedFonts = true;
        converter.Options.DisplayFooter = true;
        converter.Options.PdfPageSize = PdfPageSize.A4;

        var pdfFooter = new PdfTextSection(12, 0, 100, 50, "\n\n{page_number} / {total_pages}", new Font("Roboto-Regular", 8, FontStyle.Bold));
        converter.Footer.Add(pdfFooter);

        converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
        converter.Options.JavaScriptEnabled = false;

        var baseurl = AppDomain.CurrentDomain.BaseDirectory + "\\Content\\";

        var pdfDocument = converter.ConvertHtmlString(pdfHtml, baseurl);
        byte[] bytes = pdfDocument.Save();

        pdfDocument.Close();

        return bytes;
    }
}