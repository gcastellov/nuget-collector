using System.Collections.Generic;

namespace NugetCollector.Domain.Repositories
{
    public interface IProjectRepository
    {
        void Save(IEnumerable<Project> projects);

        IEnumerable<Project> GetProjects();
    }
}