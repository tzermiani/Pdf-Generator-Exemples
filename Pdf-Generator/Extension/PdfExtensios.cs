using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace Pdf_Generator.Extension
{
    public static class PdfExtensios
    {
        public static Table AddCellIf(this Table table, bool condicional, Cell cell)
        {
            if (condicional)
                return table.AddCell(cell);
            else
                return table;
        }
    }
}
