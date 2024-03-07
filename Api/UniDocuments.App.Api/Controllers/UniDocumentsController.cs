using Microsoft.AspNetCore.Mvc;

namespace UniDocuments.App.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UniDocumentsController : ControllerBase
{
    private readonly ILogger<UniDocumentsController> _logger;

    public UniDocumentsController(ILogger<UniDocumentsController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<int> Get()
    {
        return Enumerable.Range(0, 1);
    }
}