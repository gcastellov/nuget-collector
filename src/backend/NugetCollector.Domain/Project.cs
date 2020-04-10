using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace NugetCollector.Domain
{
    public class Project
    {
        public string Name { get; }
        public IEnumerable<Reference> References { get; set; }

        public Project(string name)
        {
            Name = name;
        }

        public static Project Analyze(string path)
        {
            var xml = new XmlDocument();
            xml.Load(path);
            var xmlReferences = xml.SelectNodes("//PackageReference");
            var name = new DirectoryInfo(path).Name;
            
            return new Project(name)
            {
                References = xmlReferences
                    .Cast<XmlNode>()
                    .Select(node => Reference.From(node))
                    .ToArray()
            };
        }
    }
}