using System.Collections.Concurrent;
using Application.Services.Abstractions;
using Domain.Models;

namespace Application.Services.Concrete;

public class QueueService : IQueueService
{
    private readonly ConcurrentQueue<FullText> _queue = new();
    public void AddToQueue(string UploadId, FullText fullText)
    {
        _queue.Enqueue(fullText);
    }

    public FullText TryGetNext()
    {
        var fullText = new FullText();
        
        _queue.TryDequeue(out fullText);
        
        return fullText;
    }
}