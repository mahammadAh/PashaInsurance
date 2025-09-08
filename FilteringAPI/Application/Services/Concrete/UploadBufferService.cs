using System.Collections.Concurrent;
using System.Text;
using Application.Services.Abstractions;
using Domain.Models;

namespace Application.Services.Concrete;

public class UploadBufferService : IUploadBufferService
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<int, string>> buffer = new();
    public void AddChunk(Chunk chunk)
    {
        if (!buffer.TryGetValue(chunk.UploadId, out var dict))
        {
            dict = new ConcurrentDictionary<int, string>();
            buffer[chunk.UploadId] = dict;
        }

        dict[chunk.ChunkIndex] = chunk.Data ?? string.Empty;
    }

    public FullText GetFullText(string UploadId)
    {
        string fullText = string.Empty;
        
       if (buffer.TryGetValue(UploadId, out var dict))
       {
           var sb = new StringBuilder();
           
           var orderedDict = dict.OrderBy(k => k.Key);

           foreach (var record in orderedDict)
           {
               sb.Append(record.Value + " ");
           }
           
           fullText = sb.ToString().TrimEnd();
           
           buffer.TryRemove(UploadId, out _);
       }
       
       return new FullText
       {
           UploadId = UploadId,
           Data = fullText
       };
       
    }
}