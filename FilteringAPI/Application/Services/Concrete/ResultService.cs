using System.Collections.Concurrent;
using Application.Services.Abstractions;

namespace Application.Services.Concrete;

public class ResultService : IResultService
{
    private readonly ConcurrentDictionary<string, string> _results = new();
    
    public void Add(string uploadId, string filteredText)
    {
        _results[uploadId] = filteredText;
    }

    public string Get(string uploadId)
    {
        string filteredText;
        _results.TryGetValue(uploadId, out filteredText!);
        return filteredText;
        
    }
}