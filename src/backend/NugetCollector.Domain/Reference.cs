using System.Xml;

namespace NugetCollector.Domain
{
    public class Reference
    {
        public string Name { get; }
        public string Version { get; }

        public Reference(string name, string version)
        {
            Name = name;
            Version = version;
        }

        public static Reference From(XmlNode xmlNode)
        {
            return new Reference(
                xmlNode.Attributes["Include"].Value,
                xmlNode.Attributes["Version"].Value);
        }

        public static bool operator ==(Reference left, Reference right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(Reference left, Reference right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Version != null ? Version.GetHashCode() : 0);
            }
        }

        protected bool Equals(Reference other)
        {
            return Name == other.Name && Version == other.Version;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            var value = obj as Reference;

            if (ReferenceEquals(value, null))
            {
                return false;
            }

            return Equals(value);
        }
    }
}