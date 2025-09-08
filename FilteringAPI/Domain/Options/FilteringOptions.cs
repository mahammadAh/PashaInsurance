namespace Domain.Options;

public class FilteringOptions
{
    public double SimilarityThreshold { get; set; }
    public string Strategy { get; set; } 
    public string[] StopWords { get; set; } 
}