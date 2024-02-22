public class PdfData
{
    public IEnumerable<MaitenceUnit>? OccurrencesAndCorrections { get; set; }
    public int OccurrencesCount { get; set; }
    public HeaderData? Header { get; set; }
    public object? SummaryAndMetrics { get; set; }
    public object? CostsAndMaterials { get; set; }
    
    
    
}