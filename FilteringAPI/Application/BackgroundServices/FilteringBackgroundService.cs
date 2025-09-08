using Application.Services.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.BackgroundServices;

public class FilteringBackgroundService : BackgroundService 

{
    private readonly IQueueService _queue;
    private readonly IFilteringService _filter;
    private readonly IResultService _results;
    private readonly ILogger<FilteringBackgroundService> _logger;

    public FilteringBackgroundService(IQueueService queue, IFilteringService filter, IResultService results, ILogger<FilteringBackgroundService> logger)
    {
        _queue = queue;
        _filter = filter;
        _results = results;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("FilteringBackgroundService started");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var job = _queue.TryGetNext();
            
            if (job != null)
            {
                _logger.LogInformation($"Processing upload id: {job.UploadId}");
                var filtered = _filter.Filter(job.Data);
                _results.Add(job.UploadId, filtered);
                _logger.LogInformation($"End filtering for upload id: {job.UploadId}");
            }
            else
            {
                await Task.Delay(10, stoppingToken);
            }
        }
        
        _logger.LogInformation("FilteringBackgroundService stopped");
    }
}