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
    
   
    private readonly int _workerCount;

    public FilteringBackgroundService(IQueueService queue, IFilteringService filter, IResultService results, ILogger<FilteringBackgroundService> logger)
    {
        _queue = queue;
        _filter = filter;
        _results = results;
        _logger = logger;
        
     
        _workerCount = Environment.ProcessorCount;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"FilteringBackgroundService started with {_workerCount} workers");
        

        var workerTasks = new List<Task>();
        

        for (int i = 0; i < _workerCount; i++)
        {
            var workerId = i + 1;
            workerTasks.Add(ProcessQueueAsync(workerId, stoppingToken));
        }
        
 
        await Task.WhenAll(workerTasks);
        
        _logger.LogInformation("FilteringBackgroundService stopped");
    }
    
    private async Task ProcessQueueAsync(int workerId, CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Worker {workerId} started");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var job = _queue.TryGetNext();
            
            if (job != null)
            {
                _logger.LogInformation($"Worker {workerId} processing upload id: {job.UploadId}");

                var filtered = await _filter.FilterAsync(job.Data);
                _results.Add(job.UploadId, filtered);
                _logger.LogInformation($"Worker {workerId} finished upload id: {job.UploadId}");
            }
            else
            {
            
                await Task.Delay(50, stoppingToken);
            }
        }
        
        _logger.LogInformation($"Worker {workerId} stopped");
    }
}