using Application.Services.Concrete;
using Domain.Options;
using Microsoft.Extensions.Options;
using Xunit;

namespace Tests;

public class FilteringServiceTests
{
    [Fact]
    public void JaroWinklerFiltering_ShouldRemoveSimilarWords()
    {
        // Arrange
        var options = new FilteringOptions
        {
            SimilarityThreshold = 0.8,
            Strategy = "Jaro-Winkler",
            StopWords = new[] { "spam", "bad", "virus" , "horrible"}
        };
        var optionsWrapper = Options.Create(options);
        var filteringService = new FilteringService(optionsWrapper);
        
        var inputText = "This is spam message with bad horible content and virus";

        // Act
        var result = filteringService.Filter(inputText);

        // Assert
        var resultWords = result.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
 
        Assert.DoesNotContain("spam", resultWords);
        Assert.DoesNotContain("bad", resultWords);
        Assert.DoesNotContain("virus", resultWords);
        Assert.DoesNotContain("horible", resultWords);
        

        Assert.Contains("This", resultWords);
        Assert.Contains("is", resultWords);
        Assert.Contains("message", resultWords);
        Assert.Contains("with", resultWords);
        Assert.Contains("content", resultWords);
        Assert.Contains("and", resultWords);
    }

    [Fact]
    public void LevenshteinFiltering_ShouldRemoveSimilarWords()
    {
        // Arrange
        var options = new FilteringOptions
        {
            SimilarityThreshold = 0.8,
            Strategy = "Levenshtein",
            StopWords = new[] { "spam", "bad", "virus", "horrible" }
        };
        var optionsWrapper = Options.Create(options);
        var filteringService = new FilteringService(optionsWrapper);
        
        var inputText = "This is spam message with bad horible content and virus";

        // Act
        var result = filteringService.Filter(inputText);

        // Assert
        var resultWords = result.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
  
        Assert.DoesNotContain("spam", resultWords);
        Assert.DoesNotContain("bad", resultWords);
        Assert.DoesNotContain("virus", resultWords);
        Assert.DoesNotContain("horible", resultWords);
        

        Assert.Contains("This", resultWords);
        Assert.Contains("is", resultWords);
        Assert.Contains("message", resultWords);
        Assert.Contains("with", resultWords);
        Assert.Contains("content", resultWords);
        Assert.Contains("and", resultWords);
    }
}
