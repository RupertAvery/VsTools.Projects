using System.Xml.Linq;

namespace VsTools.Projects
{
    public class Content : Item
    {
        public string CopyToOutputDirectory
        {
            get => GetMetadataValue();
            set => AddOrUpdateMetadataValue(value);
        }

        public string DependentUpon
        {
            get => GetMetadataValue();
            set => AddOrUpdateMetadataValue(value);
        }

        public Content(XNode node) : base(node)
        {

        }

        public Content(string include) : base(include)
        {

        }
    }
}