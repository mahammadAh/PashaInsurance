using Application.Services.Abstractions;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Presentation.Controllers;


[Route("api/")]
[ApiController]

public class ChunkController : ControllerBase 
{
    private readonly IChunkService _chunkService;
    private readonly IResultService _resultService;
    private readonly ILogger<ChunkController> _logger;


    public ChunkController(IChunkService chunkService, IResultService resultService, ILogger<ChunkController> logger)
    {
        _chunkService = chunkService;
        _resultService = resultService;
        _logger = logger;
    }

    /// <summary>
    /// Upload Chunk
    /// </summary>
    /// <param name="chunk"></param>
    /// <returns>Status</returns>
    [HttpPost("upload")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<object>> Upload(Chunk chunk)
    {
        try
        {
            await _chunkService.UploadChunkAsync(chunk);

            var status = chunk?.IsLastChunk == true ? "Accepted" : "Waiting for next chunk";
            return Ok(new { status = status });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error for chunk with upload id {chunk?.UploadId}");
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// Get Result
    /// </summary>
    /// <param name="uploadId"></param>
    /// <returns>Result</returns>
    [HttpGet("{uploadId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<string>> GetById(string uploadId)
    {
        try
        {
            
            var fullText = _resultService.Get(uploadId);

            return Ok(fullText);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting result for upload id: {uploadId}");
            return BadRequest(ex.Message);
        }
    }


}