public class Occurence
{
    public string? Cqa { get; set; }
    public string? Picture { get; set; }
    public bool IsOnlyLastCorrection { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? Requester { get; set; }
    public string? RegisterDate { get; set; }
    public string? Type { get; set; }
    public string? Priority { get; set; }
    public string? System { get; set; }
    public bool? Typical { get; set; }
    public string? Provider { get; set; }
    public string? Sla { get; set; }
    public string? SlaSolutionDeadline { get; set; }
    public string? SlaTreatmentDeadline { get; set; }
    public string? ExecutionLimit { get; set; }
    public Location? Local { get; set; }
    public IEnumerable<Correction>? Corrections { get; set; }
    
    
}