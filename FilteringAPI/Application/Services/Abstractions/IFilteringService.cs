namespace Application.Services.Abstractions;

public interface IFilteringService
{

    public Task<string> FilterAsync(string text);
}