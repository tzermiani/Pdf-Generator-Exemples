using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Layout;
using Pdf_Generator.Model;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Source;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Layout.Borders;
using System.Reflection.PortableExecutable;
using iText.IO.Image;
using Pdf_Generator.Extension;
using iText.Html2pdf;

namespace Pdf_Generator.Service
{
    public class iText7Pdf
    {
        public async Task<MemoryStream> getItext7pdf2(PdfData data)
        {
            MemoryStream ms = new MemoryStream();

            PdfWriter writer = new PdfWriter(ms);
            PdfDocument pdfDoc = new PdfDocument(writer);
            Document document = new Document(pdfDoc, PageSize.A4, true);
            writer.SetCloseStream(false);

            Paragraph header = new Paragraph("Ocorrencias")
              .SetTextAlignment(TextAlignment.CENTER)
              .SetFontSize(20);

            document.Add(header);

            Paragraph subheader = new Paragraph(DateTime.Now.ToShortDateString())
              .SetTextAlignment(TextAlignment.CENTER)
              .SetFontSize(15);
            document.Add(subheader);

            // empty line
            document.Add(new Paragraph(""));

            // Line separator
            LineSeparator ls = new LineSeparator(new SolidLine());
            document.Add(ls);

            // empty line
            document.Add(new Paragraph(""));

            // Add table containing data
            document.Add(await GetPdfTable(data));

            // Page Numbers
            int n = pdfDoc.GetNumberOfPages();
            for (int i = 1; i <= n; i++)
            {
                document.ShowTextAligned(new Paragraph(String.Format("Page " + i + " of " + n)),
                  559, 806, i, TextAlignment.RIGHT,
                  VerticalAlignment.TOP, 0);
            }

            document.Close();
            byte[] byteInfo = ms.ToArray();
            ms.Write(byteInfo, 0, byteInfo.Length);
            ms.Position = 0;

            return ms;
        }

        public async Task<MemoryStream> getItext7pdffromtemplate(PdfData data)
        {
            var src = "./Templates/title-header.pdf";
            var dest = new MemoryStream();

            var writer = new PdfWriter(dest);
            writer.SetCloseStream(false);

            //Initialize PDF document
            PdfDocument finalPdf = new PdfDocument(new PdfReader(src), writer);
            PdfAcroForm form = PdfAcroForm.GetAcroForm(finalPdf, true);
            IDictionary<String, PdfFormField> fields = form.GetAllFormFields();
            PdfFormField toSet;
            fields.TryGetValue("Empresa", out toSet);
            toSet.SetValue("James Bond");
            fields.TryGetValue("UnitsCount", out toSet);
            toSet.SetValue("English");
            fields.TryGetValue("Period", out toSet);
            toSet.SetValue("Off");
            fields.TryGetValue("OccurencesCount", out toSet);
            toSet.SetValue("Yes");




            form.FlattenFields();
            finalPdf.Close();
            byte[] byteInfo = dest.ToArray();
            dest.Write(byteInfo, 0, byteInfo.Length);
            dest.Position = 0;

            return dest;

        }

        private async Task<Table> GetPdfTable(PdfData data)
        {
            // Table
            Table table = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();

            // Headings
            Cell cellUnitName = new Cell()
               .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("Unidade"));

            Cell cellStateName = new Cell()
               .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
               .SetTextAlignment(TextAlignment.LEFT)
               .Add(new Paragraph("Estado"));

            Cell cellCityName = new Cell()
               .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("Cidade"));

            Cell cellOccurencesCount = new Cell(2, 2)
               .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
               .SetTextAlignment(TextAlignment.CENTER)
               .Add(new Paragraph("Ocorrencias"));

            table.AddCell(cellUnitName);
            table.AddCell(cellStateName);
            table.AddCell(cellCityName);
            table.AddCell(cellOccurencesCount);

            foreach (var site in data.OccurrencesAndCorrections)
            {
                Cell cUnit = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph(site.Unit));

                Cell cStateName = new Cell(1, 1)
                     .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph(site.State));

                Cell cCityName = new Cell(2, 1)
                     .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph(site.City));
                Cell cOccurences = new Cell(1, 1)
                     .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph("Occurence 123"));


                table.AddCell(cUnit);
                table.AddCell(cStateName);
                table.AddCell(cCityName);


                table.AddCell(new Cell());
                table.AddCell(new Cell());
                table.AddCell(new Cell());
                table.AddCell(new Cell());
            }

            //foreach (var item in products)
            //{
            //    Cell cId = new Cell(1, 1)
            //        .SetTextAlignment(TextAlignment.CENTER)
            //        .Add(new Paragraph(item.Id.ToString()));

            //    Cell cName = new Cell(1, 1)
            //        .SetTextAlignment(TextAlignment.LEFT)
            //        .Add(new Paragraph(item.Name));

            //    Cell cQty = new Cell(1, 1)
            //        .SetTextAlignment(TextAlignment.RIGHT)
            //        .Add(new Paragraph(item.UnitsInStock.ToString()));

            //    Cell cPrice = new Cell(1, 1)
            //        .SetTextAlignment(TextAlignment.RIGHT)
            //        .Add(new Paragraph(String.Format("{0:C2}", item.UnitPrice)));

            //    table.AddCell(cId);
            //    table.AddCell(cName);
            //    table.AddCell(cQty);
            //    table.AddCell(cPrice);
            //}

            return table;
        }

        public async Task<MemoryStream> getItext7pdf(PdfData data)
        {
            return await RenderDocument(document =>
            {
                document.Add(new Paragraph(new Text("Relatório de Ocorrências").SetFontSize(20).SetFontColor(ColorConstants.ORANGE)));

                document.Add(GetHeaderTable(data.Header));

                foreach (var (unit, index) in data.OccurrencesAndCorrections.WithIndex())
                {
                    document.Add(GetUnitDiv(unit));
                    if (index < data.OccurrencesAndCorrections.Count() - 1)
                        document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                }

                return document;
            });
        }

        private Div GetUnitDiv(MaitenceUnit unit)
        {
            Div unitDiv = new Div();

            unitDiv.Add(GetUnitIdentificationDiv(unit));

            foreach (var (occurence, idx) in unit.Occurrences.WithIndex())
            {
                unitDiv.Add(new Paragraph($"{idx + 1}/{unit.Occurrences.Count()}) Ocorrência {occurence.Cqa}"));
                unitDiv.Add(getOccureceInfoDiv(occurence));
                if (idx < unit.Occurrences.Count() - 1)
                    unitDiv.Add(new AreaBreak());
            }

            return unitDiv;
        }

        private Div getOccureceInfoDiv(Occurence occurence)
        {
            var occurenceDiv = new Div();
            int occRows = string.IsNullOrEmpty(occurence.Picture) ? 2 : 1;


            var tableInfo = new Table(UnitValue.CreatePercentArray(4))
                .UseAllAvailableWidth()
                .AddCell(new Cell(1, 4).Add(new Paragraph("DADOS DA OCORRÊNCIA")))
                .AddCell(new Cell(1, occRows).Add(new Paragraph(occurence.Description ?? "")))
                .AddCell(new Cell(1, 1).Add(new Paragraph(occurence.System ?? "")))
                .AddCell(new Cell(1, 1).Add(new Paragraph(occurence.ExecutionLimit ?? "")));

            if (!string.IsNullOrEmpty(occurence.Picture))
            {

                var picURL = occurence.Picture;

                var imgData = ImageDataFactory.Create(new Uri(picURL));
                var imgContainer = new Image(imgData);
                imgContainer.SetAutoScale(true);

                tableInfo.AddCell(new Cell(1, 1).Add(imgContainer));

            }

            occurenceDiv.Add(new Paragraph(""));
            occurenceDiv.Add(tableInfo);

            occurenceDiv.Add(new Paragraph(""));
            var occurenceLocal = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
            occurenceLocal.AddCell(new Cell(1, 4).Add(new Paragraph("Local")));
            occurenceLocal.AddCell(new Cell(1, 1).Add(new Paragraph("Area")).Add(new Paragraph(occurence.Local?.Area ?? "")));
            occurenceLocal.AddCell(new Cell(1, 1));
            occurenceLocal.AddCell(new Cell(1, 1));
            occurenceLocal.AddCell(new Cell(1, 1));

            occurenceDiv.Add(occurenceLocal);
            if (occurence.Corrections != null && occurence.Corrections.Count() > 0)
            {

                occurenceDiv.Add(new Paragraph($"Correções ({occurence.Corrections.Count()})"));

                var correctionsDiv = new Div();

                foreach (var (correction, idx) in occurence.Corrections.WithIndex())
                {
                    int correctionCols = 1;
                    if (string.IsNullOrEmpty(correction.Picture))
                        correctionCols = 2;

                    var correctionsTable = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth()
                            .AddCell(new Cell(1, 1).Add(new Paragraph("Status de correção")).Add(new Paragraph(correction.Status ?? "")))
                            .AddCell(new Cell(1, 1).Add(new Paragraph("Executor")).Add(new Paragraph(correction.Executor ?? "")))
                            .AddCell(new Cell(1, correctionCols).Add(new Paragraph("Descrição")).Add(new Paragraph(correction.Description ?? "")));
                    if (!string.IsNullOrEmpty(correction.Picture))
                    {
                      
                        var img = ImageDataFactory.Create(new Uri(correction.Picture));
                        correctionsTable.AddCell(new Cell(1, 1).Add(new Image(img).SetAutoScale(true)));
                        
                    }



                    correctionsDiv.Add(correctionsTable);
                }

                occurenceDiv.Add(correctionsDiv);
            }

            return occurenceDiv;
        }

        private Div GetUnitIdentificationDiv(MaitenceUnit unit)
        {
            Div div = new Div().SetKeepTogether(true);

            div.Add(
                new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth()
                .AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Table(UnitValue.CreatePercentArray(2))
                    .UseAllAvailableWidth()
                    .AddCell(new Cell(1, 1).Add(new Paragraph(SmallText("Unidade", ColorConstants.GRAY))))
                    .AddCell(new Cell(1, 1).Add(new Paragraph(SmallText(unit.Unit, ColorConstants.BLACK))))))
                .AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Table(UnitValue.CreatePercentArray(2))
                    .UseAllAvailableWidth()
                    .AddCell(new Cell(1, 1).Add(new Paragraph(SmallText("Estado", ColorConstants.GRAY))))
                    .AddCell(new Cell(1, 1).Add(new Paragraph(SmallText(unit.State, ColorConstants.BLACK))))))
                .AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Table(UnitValue.CreatePercentArray(2))
                    .UseAllAvailableWidth()
                    .AddCell(new Cell(1, 1).Add(new Paragraph(SmallText("Cidade", ColorConstants.GRAY))))
                    .AddCell(new Cell(1, 1).Add(new Paragraph(SmallText(unit.City, ColorConstants.BLACK))))))
                .AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Table(UnitValue.CreatePercentArray(2))
                    .UseAllAvailableWidth()
                    .AddCell(new Cell(1, 1).Add(new Paragraph(SmallText("Ocorrencias", ColorConstants.GRAY))))
                    .AddCell(new Cell(1, 1).Add(new Paragraph(SmallText(unit.Occurrences.Count().ToString(), ColorConstants.BLACK))))))
                );

            return div;

        }

        private Table GetHeaderTable(HeaderData Headers)
        {
            Table headerTable = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
            headerTable.AddCell(new Cell()
                .SetBorder(Border.NO_BORDER)
                .Add(new Table(UnitValue.CreatePercentArray(2))
                .UseAllAvailableWidth()
                .AddCell(new Cell(1, 1).Add(new Paragraph(SmallText("EMPRESA", ColorConstants.GRAY))))
                .AddCell(new Cell(1, 1).Add(new Paragraph(SmallText(Headers.companyName, ColorConstants.BLACK))))));

            headerTable.AddCell(new Cell().SetBorder(Border.NO_BORDER)
                .Add(new Table(UnitValue.CreatePercentArray(2))
                .UseAllAvailableWidth()
                .AddCell(new Cell(1, 1).Add(new Paragraph(SmallText("UNIDADE(S) DE MANUTENÇÃO", ColorConstants.GRAY))))
                .AddCell(new Cell(1, 1).Add(new Paragraph(SmallText(Headers?.units ?? "0", ColorConstants.BLACK))))));

            headerTable.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Table(UnitValue.CreatePercentArray(2))
                .UseAllAvailableWidth()
                .AddCell(new Cell(1, 1).Add(new Paragraph(SmallText("ABERTURA DA OCORRÊNCIA", ColorConstants.GRAY))))
                .AddCell(new Cell(1, 1).Add(new Paragraph(SmallText(Headers.openingOfOccurrence, ColorConstants.BLACK))))));

            headerTable.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Table(UnitValue.CreatePercentArray(2))
                .UseAllAvailableWidth()
                .AddCell(new Cell(1, 1).Add(new Paragraph(SmallText("OCORRÊNCIA", ColorConstants.GRAY))))
                .AddCell(new Cell(1, 1).Add(new Paragraph(SmallText(Headers.occurrencesCount?.ToString() ?? "0", ColorConstants.BLACK))))));
            return headerTable;
        }

        public async Task<MemoryStream> RenderDocument(Func<Document, Document> RedenBody)
        {
            MemoryStream ms = new MemoryStream();

            PdfWriter writer = new PdfWriter(ms);
            writer.SetSmartMode(true);
            PdfDocument pdfDoc = new PdfDocument(writer);
            Document document = new Document(pdfDoc, PageSize.A4.Rotate(), true);
            writer.SetCloseStream(false);

            RedenBody(document);

            //SetFooter(pdfDoc, document);

            document.Close();
            byte[] byteInfo = ms.ToArray();
            ms.Write(byteInfo, 0, byteInfo.Length);
            ms.Position = 0;

            return ms;

            static void SetFooter(PdfDocument pdfDoc, Document document)
            {
                int totalPages = pdfDoc.GetNumberOfPages();
                for (int pgNumber = 1; pgNumber <= totalPages; pgNumber++)
                {
                    var strFooter = String.Format("Page " + pgNumber + " of " + totalPages);
                    document.ShowTextAligned(new Paragraph(strFooter),
                      50, 20, pgNumber, TextAlignment.CENTER,
                      VerticalAlignment.MIDDLE, 0);
                }
            }
        }

        public Text SmallText(string text, Color? color = null)
        {
            var textObj = new Text(text).SetFontSize(6);

            if (color != null)
                textObj.SetFontColor(color);

            return textObj;

        }

        public string GetFromHtml(PdfData data)
        {
            var result = $@"
            <html>
                <head>
                    <style>
                        h1{{color: orange}}
                        @page {{
                            @bottom-right {{
                                content: ""Page "" counter(page) "" of "" counter(pages);
                        }}
                    }}
                    </style>
                </head>
                <body>
                    <h1>Relatório de Ocorrencias e Correções</h1>
                    {getEmpresaHtml(data)}
                </body>
            </html
            ";
            return result;
        }

        private string getEmpresaHtml(PdfData data)
        {
            var result = "";

            foreach (var (unit, index) in data.OccurrencesAndCorrections.WithIndex())
            {
                result += @$"
                <div>
                    <table style=""border:solid 1px black;width:100%"">
                        <tr>
                            <td>Unidade</td><td>{unit.Unit}</td>
                            <td>Estado</td><td>{unit.State}</td>
                            <td>Cidade</td><td>{unit.City}</td>
                            <td>Ocorrências</td><td>{unit.Occurrences.Count()}</td>
                        </tr>
                    </table>
                    {getOccurencesHtml(unit.Occurrences)}
                </div>
                ";
            }
            return result;

        }

        private string getOccurencesHtml(IEnumerable<Occurence> occurrences)
        {
            var result = "";
            foreach (var (occurence, index) in occurrences.WithIndex())
            {
                result += $@"
                <h3>{index + 1}/{occurrences.Count()}) Ocorrência {occurence.Cqa}<h3>
                <div style=""width:100%;"">
                    <div style=""border-bottom:solid 1px black; width:100%"">
                        DADOS DA OCORRÊNCIA
                    </div>
                    <table style=""width:100%;border-collapse: collapse;"" >
                        <tr>
                            <td style=""border:solid 1px black;max-width:66%"">{occurence.Description}</td>
                            <td style=""border:solid 1px black;"">{occurence.System}</td>
                            <td style=""border:solid 1px black;"">{occurence.ExecutionLimit}</td>
                            {(!string.IsNullOrEmpty(occurence.Picture) ? $@"<td style=""border:solid 1px black;width:200px""><img style=""width:300px;object-fit:contain;""src=""{(occurence.Picture)}"" /></td>" : "")}
                        </tr>
                    </table>
                    <br />
                    <br />
                    <div style=""width=100%;border:solid 1px black"">
                        <div style=""width:100%; background-color: lightgray;border-bottom:solid 1px black"">
                            LOCAL
                        </div>
                        <div style=""width:100%;"">
                            <p>Area</p>
                            <p>{occurence.Local?.Area ?? ""}</p>
                        </div>
                    </div>
                    {getCorrectionHtml(occurence.Corrections)}
                </div>
                {((index + 1 < occurrences.Count()) ? @$"<div style=""page-break-before: always;""></div>" : "")}";


            }
            return result;

        }
        private string getCorrectionHtml(IEnumerable<Correction>? corrections)
        {
            var result = "";
            if (corrections == null || !(corrections.Count() > 0))
                return result;
            result += $@"<h3>Correções ({corrections.Count()})</h3>";
            foreach (var (correction, index) in corrections.WithIndex())
            {
                result += $@"
                    <table style=""width:100%;border:solid 1px black;margin-bottom:30px"">
                        <tr>
                            <td>status:
                                <br />
                                {correction.Status}
                            </td>
                            <td>Executor/Responsavel:
                                <br />
                                {correction.Status}
                                {correction.Responsible}
                                {correction.ResponsibleForApproval}
                            </td>
                            <td>Descrição:
                                <br />
                                {correction.Description}
                            </td>
                           {(!string.IsNullOrEmpty(correction.Picture) ? $@"<td style=""border:solid 1px black;width:200px""><img style=""width:300px;object-fit:contain;""src=""{(correction.Picture)}"" /></td>" : "")}
                    
                    
                        </tr>
                    </table>
                ";
            }

            return result;
        }
    }
}
