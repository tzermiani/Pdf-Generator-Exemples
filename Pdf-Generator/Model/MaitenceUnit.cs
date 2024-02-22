public class MaitenceUnit
{
    public string? Unit { get; set; }
    public string? State { get; set; }
    public string? City { get; set; }
    public IEnumerable<Occurence>? Occurrences { get; set; }
}