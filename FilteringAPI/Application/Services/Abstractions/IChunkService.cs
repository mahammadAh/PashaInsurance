using Domain.Models;

namespace Application.Services.Abstractions;

public interface IChunkService
{
    Task UploadChunkAsync(Chunk chunk);
}