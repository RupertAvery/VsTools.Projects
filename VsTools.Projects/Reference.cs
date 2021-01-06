using System.Xml.Linq;

namespace VsTools.Projects
{
    public class Reference : Item
    {
        public string HintPath
        {
            get => GetMetadataValue();
            set => AddOrUpdateMetadataValue(value);
        }

        public string Private
        {
            get => GetMetadataValue();
            set => AddOrUpdateMetadataValue(value);
        }

        public string SpecificVersion
        {
            get => GetMetadataValue();
            set => AddOrUpdateMetadataValue(value);
        }

        public Reference(XNode node) : base(node)
        {
        }

        public Reference(string include) : base(include)
        {
        }

    }
}