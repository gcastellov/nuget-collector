﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LibGit2Sharp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NugetCollector.Api.Configuration;
using NugetCollector.Api.Hubs;
using NugetCollector.Domain;
using NugetCollector.Domain.Repositories;
using Credentials = LibGit2Sharp.Credentials;

namespace NugetCollector.Api.Controllers
{
    [ApiController]
    public class CodeController : Controller
    {
        private readonly CollectorOptions _options;
        private readonly ILogger<CodeController> _logger;
        private readonly IProjectRepository _projectRepository;
        private readonly ICodeHubSender _hubSender;

        public CodeController(
            ILogger<CodeController> logger,
            IOptions<CollectorOptions> options,
            IProjectRepository projectRepository,
            ICodeHubSender hubContext)
        {
            _options = options.Value;
            _logger = logger;
            _projectRepository = projectRepository;
            _hubSender = hubContext;
        }

        [Route("/api/analyze")]
        [HttpPost]
        public HttpResponseMessage Analyze()
        {
            _logger.LogDebug("Analyzing repositories");

            _hubSender.SendMessage("Synchronizing all repositories");

            var executingTasks = new List<Task>();
            foreach (var codeRepository in _options.Repositories)
            {
                var task = UpdateRepository(codeRepository);
                executingTasks.Add(task);
            }

            var taskFactory = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);
            taskFactory.ContinueWhenAll(executingTasks.ToArray(), tasks => AnalyzeProjects());
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        private void AnalyzeProjects()
        {
            _hubSender.SendMessage("Analyzing all projects");

            var projects = new ProjectLookUp(_options.LocalPath)
                .Find()
                .Select(Project.Analyze)
                .ToArray();

            _projectRepository.Save(projects);

            _hubSender.SendMessage("Done!");
        }

        private async Task UpdateRepository(CodeRepository codeRepository)
        {
            var path = GetLocalPath(codeRepository);

            if (Directory.Exists(path))
            {
                await Pull(codeRepository, path);
            }
            else
            {
                await Clone(codeRepository, path);
            }
        }

        private async Task Clone(CodeRepository codeRepository, string path)
        {
            try
            {
                await _hubSender.SendStatus(codeRepository.Name, "Cloning  repository");
                CloneRepository(codeRepository, path);

                _logger.LogDebug($"{codeRepository.Name} has been cloned into {path}");
                await _hubSender.SendStatus(codeRepository.Name, "Repository up to date");
            }
            catch (Exception e)
            {
                _logger.LogError($"An exception has been thrown cloning {codeRepository.Name}", e);
                await _hubSender.SendStatus(codeRepository.Name, "Repository faulted");
            }
        }

        private async Task Pull(CodeRepository codeRepository, string path)
        {
            try
            {
                await _hubSender.SendStatus(codeRepository.Name, "Pulling repository");
                PullRepository(path);

                _logger.LogDebug($"{codeRepository.Name} has been pulled into {path}");
                await _hubSender.SendStatus(codeRepository.Name, "Repository up to date");
            }
            catch (Exception e)
            {
                _logger.LogError($"An exception has been thrown pulling {codeRepository.Name}", e);
                await _hubSender.SendStatus(codeRepository.Name, "Repository faulted");
            }
        }

        private string GetLocalPath(CodeRepository codeRepository) 
            => Path.Combine(_options.LocalPath, codeRepository.Name);

        private Credentials GetCredentials()
        {
            if (_options.ViaHttp)
            {
                return new UsernamePasswordCredentials
                {
                    Username = _options.Credential.Username,
                    Password = _options.Credential.Password
                };
            }

            return new DefaultCredentials();
        }

        private void CloneRepository(CodeRepository codeRepository, string localPath)
        {
            var options = new CloneOptions
            {
                CredentialsProvider = (url, user, cred) => GetCredentials()
            };

            Repository.Clone(codeRepository.Path, localPath, options);
        }

        private void PullRepository(string localPath)
        {
            using (var repo = new Repository(localPath))
            {
                var options = new PullOptions
                {
                    FetchOptions = new FetchOptions
                    {
                        CredentialsProvider = (url, usernameFromUrl, types) => GetCredentials()
                    }
                };

                var signature = new Signature(new Identity(_options.Credential.Username, _options.Credential.Email), DateTimeOffset.Now);

                Commands.Pull(repo, signature, options);
            }
        }
    }
}