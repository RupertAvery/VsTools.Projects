using System.Xml.Linq;

namespace VsTools.Projects
{
    public class ProjectReference : Item
    {
        public string Name
        {
            get => GetMetadataValue();
            set => AddOrUpdateMetadataValue(value);
        }

        public string Project
        {
            get => GetMetadataValue();
            set => AddOrUpdateMetadataValue(value);
        }

        public string Package
        {
            get => GetMetadataValue();
            set => AddOrUpdateMetadataValue(value);
        }

        public ProjectReference(XNode node) : base(node)
        {
        }

        public ProjectReference(string include) : base(include)
        {
        }
    }
}