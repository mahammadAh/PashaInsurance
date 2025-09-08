namespace Application.Services.Abstractions;

public interface IResultService
{
     void Add(string uploadId, string filteredText);

     string Get(string uploadId);
}