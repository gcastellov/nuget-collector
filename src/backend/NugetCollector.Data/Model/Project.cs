using System.Collections.Generic;

namespace NugetCollector.Data.Model
{
    public class Project
    {
        public Project()
        {
            ProjectReferences = new List<ProjectReference>();
        }

        public string Id { get; set; }
        public ICollection<ProjectReference> ProjectReferences { get; set; }
    }
}