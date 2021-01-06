using System.Xml.Linq;

namespace VsTools.Projects
{
    public class EmbeddedResource : Item
    {
        public string LogicalName
        {
            get => GetMetadataValue();
            set => AddOrUpdateMetadataValue(value);
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

        public EmbeddedResource()
        {
        }

        public EmbeddedResource(XNode node) : base(node)
        {
        }

        public EmbeddedResource(string include) : base(include)
        {
        }

        //public Property GetMetadataElement([CallerMemberName] string name = null)
        //{
        //    var element = Element.Elements(name).FirstOrDefault();
        //    return element != null ? new Property(element) : null;
        //}

        //public void SetMetadataElement(Property element, [CallerMemberName] string name = null)
        //{
        //    var existing = Element.Elements(name).FirstOrDefault();
        //    existing?.Remove();
        //    Element.Add(element.Node);
        //}

    }
}