using Microsoft.AspNetCore.Mvc;

namespace EveGankFeed.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ZKillController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ZKillController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("ZKillFeed")]
    public async Task<IActionResult> ZKillFeed()
    {
        var client = _httpClientFactory.CreateClient();

        try
        {
            var response = await client.GetAsync("https://zkillredisq.stream/listen.php?queueID=GankFeed&ttw=1");
            var responseContent = await response.Content.ReadAsStringAsync();

            return Content(responseContent, "application/json");
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"{ex.Message}");
        }
    }
}
