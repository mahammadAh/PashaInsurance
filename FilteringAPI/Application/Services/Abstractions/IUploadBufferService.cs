using Domain.Models;

namespace Application.Services.Abstractions;

public interface IUploadBufferService
{ 
    void AddChunk(Chunk chunk); 
    FullText GetFullText(string UploadId);
}