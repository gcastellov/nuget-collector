using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NugetCollector.Domain.Repositories;

namespace NugetCollector.Api.Controllers
{
    [ApiController]
    public class ProjectController : Controller
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly IProjectRepository _projectRepository;

        public ProjectController(ILogger<ProjectController> logger, IProjectRepository projectRepository)
        {
            _logger = logger;
            _projectRepository = projectRepository;
        }

        [Route("/api/projects")]
        [HttpGet]
        public IActionResult GetByProject()
        {
            _logger.LogDebug("Getting by projects");

            var projects = _projectRepository.GetProjects();
            return Ok(projects);
        }

        [Route("/api/references")]
        [HttpGet]
        public IActionResult GetByReferences()
        {
            _logger.LogDebug("Getting by references");

            var projects = _projectRepository.GetProjects();
            var byReference = projects.SelectMany(p => p.References.Select(r => new { p, r }))
                .GroupBy(r => r.r.Name)
                .Select(r => new
                {
                    Reference = r.Key,
                    Matches = r.Select(k => new { k.r.Version, k.p.Name })
                });

            return Ok(byReference);
        }

        [Route("/api/versions")]
        [HttpGet]
        public IActionResult GetByVersions()
        {
            _logger.LogDebug("Getting by versions");

            var projects = _projectRepository.GetProjects();
            var byVersion = projects.SelectMany(p => p.References.Select(r => new { p, r }))
                .GroupBy(r => r.r.Name)
                .Select(r => new
                {
                    Reference = r.Key,
                    Matches = r.GroupBy(k => k.r.Version)
                        .Select(v => new
                        {
                            Version = v.Key,
                            Matches = v.Select(m => m.p.Name)
                        })
                });

            return Ok(byVersion);
        }

        [Route("/api/stats")]
        [HttpGet]
        public IActionResult GetStats()
        {
            _logger.LogDebug("Getting stats");

            var projects = _projectRepository.GetProjects().ToArray();
            var allReferences = projects.SelectMany(p => p.References)
                .Distinct()
                .OrderBy(r => r.Name)
                .ToArray();
            
            var differentReferences = allReferences.Select(r => r.Name)
                .Distinct()
                .Count();

            var stats = new
            {
                ProjectsCount = projects.Length,
                AllReferenceCount = projects.SelectMany(p => p.References).Count(),
                UniqueReferenceCount = allReferences.Length,
                DifferentReferenceCount = differentReferences
            };

            return Ok(stats);
        }
    }
}