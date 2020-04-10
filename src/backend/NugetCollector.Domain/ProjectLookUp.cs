using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NugetCollector.Domain
{
    public class ProjectLookUp
    {
        private static readonly string[] DirectoryExceptions =
        {
            ".git",
            ".nuget",
            ".vs",
            "obj",
            "bin",
            "debug",
            "release",
            "packages"
        };

        private readonly string _path;

        public ProjectLookUp(string path)
        {
            _path = path;
        }

        public IEnumerable<string> Find()
        {
            var projects = new List<string>();
            GetProjectFiles(_path, projects);
            projects.Sort();
            return projects;
        }

        private static void GetProjectFiles(string directory, List<string> projects)
        {
            FindProjects(directory, projects);

            var directories = Directory.GetDirectories(directory)
                .Select(d => d.ToLowerInvariant())
                .Where(d => !DirectoryExceptions.Any(e => d.Contains(e)))
                .ToArray();

            foreach (var path in directories)
            {
                GetProjectFiles(path, projects);
            }
        }

        private static void FindProjects(string directory, List<string> projects)
        {
            var files = Directory.GetFileSystemEntries(directory, "*.csproj").ToArray();
            if (files.Any())
            {
                projects.AddRange(files);
            }
        }
    }
}