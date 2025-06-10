using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace EveGankFeed.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ESIController : Controller
{

    private readonly IHttpClientFactory _httpClientFactory;

    public ESIController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Gets IDs for an array of char names
    /// </summary>
    /// <param name="names"></param>
    /// <returns></returns>
    [HttpPost("CharacterId")]
    public async Task<IActionResult> CharacterId([FromBody] List<string> names)
    {
        var client = _httpClientFactory.CreateClient();

        var json = JsonSerializer.Serialize(names);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PostAsync("https://esi.evetech.net/latest/universe/ids/?datasource=tranquility&language=en", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            return Content(responseContent, "application/json");
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"{ex.Message}");
        }
    }

    [HttpGet("Systems")]
    public async Task<IActionResult> AllSystemIds()
    {
        var client = _httpClientFactory.CreateClient();

        try
        {
            var response = await client.GetAsync("https://esi.evetech.net/latest/universe/systems/?datasource=tranquility");
            var responseContent = await response.Content.ReadAsStringAsync();

            return Content(responseContent, "application/json");
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"{ex.Message}");
        }
    }

}
