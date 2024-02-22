using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

namespace Pdf_Generator.Service
{
    public class QuestPdfService
    {
        public  byte[] GetSimpleQuestPdf()
        {
            Document doc = Document.Create(container => 
            {
                container.Page(page =>
                {
                    page.Margin(1.5f, Unit.Centimetre);
                    page.MarginBottom(150, Unit.Millimetre);
                    page.Size(PageSizes.A4.Portrait());

                    page.Header();

                    page.Content().Column(column =>
                    {
                        column.Item().Text("Loren Ipsum");
                    });

                    page.Footer();
                });
            });

            doc.ShowInPreviewerAsync();

            return doc.GeneratePdf();
        }
        internal byte[] GetFromQuestPdf(PdfData data)
        {
            Document? doc = null;

            doc = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(1, Unit.Centimetre);
                    page.Size(PageSizes.A4);

                    page.Content()
                        .Column(column =>
                        {
                            column.Item()
                                .Text("Cabeçalho Relatório");
                            foreach (var unit in data.OccurrencesAndCorrections!)
                            {
                                column.Item()
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Darken4)
                                    .Row(row =>
                                    {
                                        row.Spacing(5);

                                        row.RelativeItem(3)
                                            .BorderRight(1)
                                            .BorderColor(Colors.Grey.Darken1)
                                            .Text(text =>
                                            {
                                                text.DefaultTextStyle(style => style.FontSize(8));
                                                text.Span("Unidade: ")
                                                    .FontColor(Colors.Grey.Medium);
                                                text.Span(unit.Unit)
                                                    .FontColor(Colors.Black);
                                            });

                                        row.RelativeItem()
                                            .BorderRight(1)
                                            .BorderColor(Colors.Grey.Darken1)
                                            .Text(text =>
                                            {
                                                text.DefaultTextStyle(style => style.FontSize(8));
                                                text.Span("Estado: ")
                                                    .FontColor(Colors.Grey.Medium);
                                                ;
                                                text.Span(unit.State)
                                                    .FontColor(Colors.Black);
                                            });
                                        row.RelativeItem()
                                            .BorderRight(1)
                                            .BorderColor(Colors.Grey.Darken1)
                                            .Text(text =>
                                            {
                                                text.DefaultTextStyle(style => style.FontSize(8));
                                                text.Span("Cidade: ")
                                                    .FontColor(Colors.Grey.Medium);
                                                ;
                                                text.Span(unit.City)
                                                    .FontColor(Colors.Black);
                                            });
                                        row.RelativeItem()
                                            .BorderRight(1)
                                            .BorderColor(Colors.Grey.Darken1)
                                            .Text(text =>
                                            {
                                                text.DefaultTextStyle(style => style.FontSize(8));
                                                text.Span("Ocorrências: ")
                                                    .FontColor(Colors.Grey.Medium);
                                                ;
                                                text.Span(unit?.Occurrences?.Count().ToString())
                                                    .FontColor(Colors.Black);
                                            });
                                    });
                                foreach (var occurrence in unit.Occurrences)
                                {
                                    column.Spacing(10);
                                    column.Item()
                                        .Column(column => { column.Item().Text($"Ocorrencia : {occurrence.Cqa}").Bold(); });
                                    column.Item()
                                        .Border(1)
                                        .BorderColor(Colors.Grey.Lighten1)
                                        .Column(column =>
                                        {
                                            column.Item()
                                                .Background(Colors.Grey.Lighten4)
                                                .Text("Dados da Ocorrênca")
                                                .Bold();
                                            column.Item()
                                                .Row(row =>
                                                {
                                                    row.RelativeItem()
                                                        .Border(1)
                                                        .BorderColor(Colors.Grey.Lighten1)
                                                        .Column(col =>
                                                        {
                                                            col.Spacing(2);
                                                            col.Item()
                                                                .Padding(2)
                                                                .Text("Descrição")
                                                                .FontSize(8)
                                                                .Bold();
                                                            col.Item()
                                                                .Padding(2)
                                                                .Text(occurrence.Description)
                                                                .FontSize(6);

                                                            col.Item()
                                                                .Column(col =>
                                                                {
                                                                    col.Item()
                                                                        .Row(row =>
                                                                        {
                                                                            row.RelativeItem()
                                                                                .BorderRight(1)
                                                                                .BorderColor(Colors.Grey.Lighten4)
                                                                                .Column(col =>
                                                                                {
                                                                                    col.Item()
                                                                                        .Text("Status da ocorréncia")
                                                                                        .FontSize(6)
                                                                                        .Bold();
                                                                                    col.Item()
                                                                                        .Text(occurrence.Status)
                                                                                        .FontSize(6);
                                                                                    col.Item()
                                                                                        .Text("Data hora cadastro")
                                                                                        .FontSize(6)
                                                                                        .Bold();
                                                                                    col.Item()
                                                                                        .Text(occurrence.RegisterDate)
                                                                                        .FontSize(6);
                                                                                });
                                                                            row.RelativeItem()
                                                                                .Column(col =>
                                                                                {
                                                                                    col.Item()
                                                                                        .Text("Solicitante")
                                                                                        .FontSize(6)
                                                                                        .Bold();
                                                                                    col.Item()
                                                                                        .Text(occurrence.Requester)
                                                                                        .FontSize(6);
                                                                                    col.Item()
                                                                                        .Text("Tipo de ocorréncia")
                                                                                        .FontSize(6)
                                                                                        .Bold();
                                                                                    col.Item()
                                                                                        .Text(occurrence.Type)
                                                                                        .FontSize(6);
                                                                                });
                                                                        });
                                                                    col.Item()
                                                                        .Column(col =>
                                                                        {
                                                                            col.Item()
                                                                                .Text("Prioridade da Ocorrência")
                                                                                .FontSize(8)
                                                                                .Bold();
                                                                            col.Item()
                                                                                .Text(occurrence.Priority)
                                                                                .FontSize(8);
                                                                        });
                                                                });
                                                        });
                                                    row.RelativeItem()
                                                        .Border(1)
                                                        .BorderColor(Colors.Grey.Lighten1)
                                                        .Column(col =>
                                                        {
                                                            col.Item()
                                                                .Text("Sistema")
                                                                .Bold()
                                                                .FontSize(6);
                                                            col.Item()
                                                                .Text(occurrence.System)
                                                                .FontSize(6);
                                                        });
                                                    row.RelativeItem()
                                                        .Border(1)
                                                        .BorderColor(Colors.Grey.Lighten1)
                                                        .Column(col =>
                                                        {
                                                            col.Item()
                                                                .Text("Limite para execução")
                                                                .FontSize(6)
                                                                .Bold();
                                                            col.Item()
                                                                .Text(occurrence.ExecutionLimit)
                                                                .FontSize(6);
                                                        });
                                                    if (!string.IsNullOrEmpty(occurrence.Picture))
                                                    {
                                                        row.RelativeItem()
                                                            .Border(1)
                                                            .BorderColor(Colors.Grey.Lighten1)
                                                            .Column(column =>
                                                            {
                                                                column.Item()
                                                                    .Text("foto")
                                                                    .FontSize(6)
                                                                    .Bold();
                                                                column.Item().ImageFromUrl(occurrence.Picture);
                                                            });
                                                    }
                                                });
                                        });

                                    if (occurrence.Local is not null)
                                    {
                                        column.Item()
                                            .Border(1)
                                            .BorderColor(Colors.Grey.Lighten4)
                                            .Column(col =>
                                            {
                                                col.Item()
                                                    .Background(Colors.Grey.Lighten4)
                                                    .BorderColor(Colors.Grey.Lighten1)
                                                    .Border(1)
                                                    .Text("Local");

                                                col.Item()
                                                    .Border(1)
                                                    .BorderColor(Colors.Grey.Lighten1)
                                                    .Row(row =>
                                                    {
                                                        if (!string.IsNullOrEmpty(occurrence.Local.AreaGroup))
                                                            row.RelativeItem()
                                                                .BorderColor(Colors.Grey.Lighten1)
                                                                .Border(1)
                                                                .Column(col =>
                                                                {
                                                                    col.Item()
                                                                        .Text("Grupo de Área: ")
                                                                        .Bold()
                                                                        .FontSize(6);
                                                                    col.Item()
                                                                        .Text(occurrence.Local.AreaGroup)
                                                                        .FontSize(6);
                                                                });
                                                        if (!string.IsNullOrEmpty(occurrence.Local.AreaSubgroup))
                                                            row.RelativeItem()
                                                                .BorderColor(Colors.Grey.Lighten1)
                                                                .Border(1)
                                                                .Column(col =>
                                                                {
                                                                    col.Item()
                                                                        .Text("Subgrupo de Área: ")
                                                                        .Bold()
                                                                        .FontSize(6);
                                                                    col.Item()
                                                                        .Text(occurrence.Local.AreaSubgroup)
                                                                        .FontSize(6);
                                                                });
                                                        if (!string.IsNullOrEmpty(occurrence.Local.Area))
                                                            row.AutoItem()
                                                                .BorderColor(Colors.Grey.Lighten1)
                                                                .Border(1)
                                                                .Column(col =>
                                                                {
                                                                    col.Item()
                                                                        .Text("Área: ")
                                                                        .Bold()
                                                                        .FontSize(6);
                                                                    col.Item()
                                                                        .Text(occurrence.Local.Area)
                                                                        .FontSize(6);
                                                                });
                                                    });
                                            });
                                    }

                                    if (occurrence.Corrections is not null)
                                    {
                                        column.Item()
                                            .Text($"Correções ({occurrence.Corrections.Count().ToString()})")
                                            .Bold();

                                        foreach (var (correction, index) in occurrence.Corrections.Reverse().WithIndex())
                                        {
                                            column.Item()
                                                .Border(1)
                                                .BorderColor(Colors.Grey.Lighten1)
                                                .Row(row =>
                                                {
                                                    row.RelativeItem(1)
                                                        .BorderColor(Colors.Grey.Lighten1)
                                                        .BorderRight(1)
                                                        .Column(column =>
                                                        {
                                                            column.Item()
                                                                .Text("Status de correção")
                                                                .Bold()
                                                                .FontSize(8);
                                                            column.Item()
                                                                .Text(correction.Status)
                                                                .FontSize(8)
                                                                .Bold()
                                                                .FontColor(Colors.Orange.Lighten1);

                                                            if (!string.IsNullOrEmpty(correction.StartDate))
                                                            {
                                                                column.Item()
                                                                    .Text("Inicio")
                                                                    .Bold()
                                                                    .FontSize(8);
                                                                column.Item()
                                                                    .Text(correction.StartDate)
                                                                    .FontSize(8);
                                                            }

                                                            if (!string.IsNullOrEmpty(correction.EndDate))
                                                            {
                                                                column.Item()
                                                                    .Text("Término")
                                                                    .Bold()
                                                                    .FontSize(8);
                                                                column.Item()
                                                                    .Text(correction.EndDate)
                                                                    .FontSize(8);
                                                            }

                                                            if (!string.IsNullOrEmpty(correction.DateTime))
                                                            {
                                                                column.Item()
                                                                    .Text("Data/Hora")
                                                                    .Bold()
                                                                    .FontSize(8);
                                                                column.Item()
                                                                    .Text(correction.DateTime)
                                                                    .FontSize(8);
                                                            }

                                                            if (!string.IsNullOrEmpty(correction.Requester))
                                                            {
                                                                column.Item()
                                                                    .Text("Solicitante")
                                                                    .Bold()
                                                                    .FontSize(8);
                                                                column.Item()
                                                                    .Text(correction.Requester)
                                                                    .FontSize(8);
                                                            }
                                                        });
                                                    row.RelativeItem(1)
                                                        .BorderColor(Colors.Grey.Lighten1)
                                                        .BorderRight(1)
                                                        .Column(column =>
                                                        {
                                                            if (!string.IsNullOrEmpty(correction.Executor))
                                                            {
                                                                column.Item()
                                                                    .Text("Executor")
                                                                    .Bold()
                                                                    .FontSize(8);
                                                                column.Item()
                                                                    .Text(correction.Executor)
                                                                    .FontSize(8);
                                                            }

                                                            if (!string.IsNullOrEmpty(correction.CorrectionTime))
                                                            {
                                                                column.Item()
                                                                    .Text("Tempo correção")
                                                                    .Bold()
                                                                    .FontSize(8);
                                                                column.Item()
                                                                    .Text(correction.CorrectionTime)
                                                                    .FontSize(8);
                                                            }

                                                            if (!string.IsNullOrEmpty(correction.Responsible))
                                                            {
                                                                column.Item()
                                                                    .Text("Responsável")
                                                                    .Bold()
                                                                    .FontSize(8);
                                                                column.Item()
                                                                    .Text(correction.Responsible)
                                                                    .FontSize(8);
                                                            }
                                                        });
                                                    row.RelativeItem(2)
                                                        .BorderColor(Colors.Grey.Lighten1)
                                                        .BorderRight(1)
                                                        .Column(column =>
                                                        {
                                                            column.Item()
                                                                .Text("Descrição")
                                                                .FontSize(8)
                                                                .Bold();
                                                            column.Item()
                                                                .Text(correction.Description)
                                                                .FontSize(8);
                                                        });
                                                    if (!string.IsNullOrEmpty(correction.Picture))
                                                    {
                                                        row.RelativeItem(1)
                                                            .BorderColor(Colors.Grey.Lighten1)
                                                            .BorderRight(1)
                                                            .Column(column =>
                                                            {
                                                                column.Item()
                                                                    .Text("foto")
                                                                    .FontSize(6)
                                                                    .Bold();
                                                                column.Item().ImageFromUrl(correction.Picture);
                                                            });
                                                    }
                                                });
                                        }
                                    }

                                    column.Item().PageBreak();
                                }
                            }
                        });
                    //page.Footer()
                    //    .AlignCenter()
                    //    .Text(text =>
                    //    {
                    //        text.DefaultTextStyle(textStyle => textStyle.FontSize(16));
                    //        text.Span("Página")
                    //            .Bold();
                    //        text.CurrentPageNumber();
                    //        text.Span("/");
                    //        text.TotalPages();
                    //    });
                });
            });

            return doc.GeneratePdf();
        }

    }
}
