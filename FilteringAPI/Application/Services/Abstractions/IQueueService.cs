using Domain.Models;

namespace Application.Services.Abstractions;

public interface IQueueService
{
    void AddToQueue(string UploadId, FullText fullText);
    
    FullText TryGetNext();
    
}