using System.Xml.Linq;

namespace VsTools.Projects
{
    public class Page : Item
    {
        public string SubType
        {
            get => GetMetadataValue();
            set => AddOrUpdateMetadataValue(value);
        }

        public string Generator
        {
            get => GetMetadataValue();
            set => AddOrUpdateMetadataValue(value);
        }

        public Page()
        {

        }

        public Page(XNode node) : base(node)
        {

        }

        public Page(string include) : base(include)
        {

        }

    }
}