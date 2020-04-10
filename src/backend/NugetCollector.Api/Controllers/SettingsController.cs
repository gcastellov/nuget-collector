using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NugetCollector.Api.Configuration;

namespace NugetCollector.Api.Controllers
{
    [ApiController]
    public class SettingsController : Controller
    {
        private CollectorOptions _options;

        public SettingsController(IOptions<CollectorOptions> options)
        {
            _options = options.Value;
        }

        [Route("/api/settings")]
        [HttpGet]
        public IActionResult GetSettings()
        {
            var dto = new
            {
                _options.LocalPath,
                Repositories = _options.Repositories.Select(r => new 
                {
                    IsMapped = Directory.Exists(Path.Combine(_options.LocalPath, r.Name)),
                    r.Name,
                    r.Path
                })
            };

            return Ok(dto);
        }
    }
}