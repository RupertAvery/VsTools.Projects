using System.Xml.Linq;

namespace VsTools.Projects
{
    public class PackageReference : Item
    {
        public string Version
        {
            get => GetMetadataValue();
            set => AddOrUpdateMetadataValue(value);
        }

        public PackageReference(XNode node) : base(node)
        {
        }

        public PackageReference(string include, string version) : base(include)
        {
            Version = version;
        }
    }
}