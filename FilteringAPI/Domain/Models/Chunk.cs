namespace Domain.Models;

public class Chunk
{
    public string UploadId { get; set; }
    public int ChunkIndex { get; set; }
    public string Data { get; set; }
    public bool IsLastChunk { get; set; }
}