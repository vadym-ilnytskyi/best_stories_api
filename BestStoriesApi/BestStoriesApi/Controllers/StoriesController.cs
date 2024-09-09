using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace BestStoriesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StoriesController(IDistributedCache cache) : ControllerBase
{

    [HttpGet("best/{number}")]
    public async Task<ActionResult> Get(int number)
    {
        return Ok(await cache.GetStringAsync($"beststories:{number}"));
    }
}