using System.Collections.Generic;

namespace NugetCollector.Api.Configuration
{
    public class CollectorOptions
    {
        public string LocalPath { get; set; }
        public bool ViaHttp { get; set; }
        public Credential Credential { get; set; }
        public IEnumerable<CodeRepository> Repositories { get; set; }
    }
}