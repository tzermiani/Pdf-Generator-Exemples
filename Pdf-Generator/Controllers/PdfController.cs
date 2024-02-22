using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Pdf_Generator.Model;
using Pdf_Generator.Service;
using iText.Html2pdf;
using System.Text;

namespace Pdf_Generator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PdfController : ControllerBase
    {
        [HttpPost("from-itext")]
        public async Task<IActionResult> GetItextPdf(PdfData data)
        {

            var ms = await new iText7Pdf().getItext7pdf(data);
            FileStreamResult fileStreamResult = new FileStreamResult(ms, "application/pdf");

            //Uncomment this to return the file as a download
            fileStreamResult.FileDownloadName = "Ocorrencias.pdf";

            return fileStreamResult;
        }
        [HttpPost("from-template")]
        public async Task<IActionResult>FromTemplate(PdfData data)
        {
            var ms = await new iText7Pdf().getItext7pdffromtemplate(data);
            FileStreamResult fileStreamResult = new FileStreamResult(ms, "application/pdf");

            //Uncomment this to return the file as a download
            fileStreamResult.FileDownloadName = "Ocorrencias-from-template.pdf";

            return fileStreamResult;


        }
        [HttpPost("from-html")]
        public async Task<IActionResult> FromHtml(PdfData data)
        {
            using (Stream htmlSource = new MemoryStream(Encoding.UTF8.GetBytes(new iText7Pdf().GetFromHtml(data))))
            using (MemoryStream stream = new MemoryStream())
            {
                PdfDocument pdfDocument = new PdfDocument(new PdfWriter(stream));
                pdfDocument.SetDefaultPageSize(PageSize.A4.Rotate());
                ConverterProperties converterProperties = new ConverterProperties();
                HtmlConverter.ConvertToPdf(htmlSource, pdfDocument, converterProperties);

                return File(stream.ToArray(), "application/pdf", "Occurences-from-html.pdf");
            }
        }
        [HttpPost("from-QuestPdf")]
        public async Task<IActionResult> FromQuestPdf(PdfData data)
        {
            return File(new QuestPdfService().GetFromQuestPdf(data), "application/pdf", "Occurences-from-questPdf.pdf");
        }
        [HttpGet("simple-quest-pdf")]
        public async Task<IActionResult> SimpleQuestPdf()
        {
            return File(new QuestPdfService().GetSimpleQuestPdf(), "application/pdf", "simple-from-questPdf.pdf");
        }

        [HttpGet("from-poc-gerador-pdf")]
        public async Task<IActionResult> FromPocGerador()
        {
            var fileName = "./templates/poc-gerador-pdf.html";
            using Stream htmlSource = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read);
            using MemoryStream stream = new MemoryStream();
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(stream));
            pdfDocument.SetDefaultPageSize(PageSize.A4.Rotate());
            ConverterProperties converterProperties = new ConverterProperties();
            HtmlConverter.ConvertToPdf(htmlSource, pdfDocument, converterProperties);

            return File(stream.ToArray(), "application/pdf", "Occurences-from-html.pdf");
        }
    }
}
