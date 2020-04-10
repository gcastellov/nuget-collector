using System;

namespace NugetCollector.Data.Model
{
    public class ProjectReference
    {
        public Guid Id { get; set; }
        public Project Project { get; set; }
        public Reference Reference { get; set; }
    }
}