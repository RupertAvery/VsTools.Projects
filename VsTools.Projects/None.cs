using System.Xml.Linq;

namespace VsTools.Projects
{
    public class None : Item
    {
        public string Update
        {
            get => GetAttributeValue();
            set => AddOrUpdateAttribute(value);
        }

        public string DependentUpon
        {
            get => GetMetadataValue();
            set => AddOrUpdateMetadataValue(value);
        }

        public string CopyToOutputDirectory
        {
            get => GetMetadataValue();
            set => AddOrUpdateMetadataValue(value);
        }

        public None()
        {

        }

        public None(XNode node) : base(node)
        {

        }

        public None(string include) : base(include)
        {

        }

    }
}