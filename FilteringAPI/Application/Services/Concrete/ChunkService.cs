using Application.Services.Abstractions;
using Domain.Models;

namespace Application.Services.Concrete;

public class ChunkService : IChunkService
{
    
    private readonly IUploadBufferService _bufferService;
    private readonly IQueueService _queueService;

    public ChunkService(IUploadBufferService bufferService, IQueueService queueService)
    {
        _bufferService = bufferService;
        _queueService = queueService;
    }
    public Task UploadChunkAsync(Chunk chunk)
    {
        _bufferService.AddChunk(chunk);

        if (chunk.IsLastChunk)
        {
            var fullText =  _bufferService.GetFullText(chunk.UploadId);
            _queueService.AddToQueue(chunk.UploadId, fullText);
        }
        
        return Task.CompletedTask;
    }
}