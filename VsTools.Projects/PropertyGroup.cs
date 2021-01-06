using System.Linq;
using System.Xml.Linq;

namespace VsTools.Projects
{
    public class PropertyGroup : ProjectChildElement
    {
        private readonly MetadataCollection _propertyCollection ;
        
        public PropertyGroup()
        {
            _propertyCollection = new MetadataCollection(this);
        }

        public PropertyGroup(XNode node) : base(node)
        {
        }

        public bool HasProperty(string property)
        {
            return HasMetadata(property);
        }

        public string GetProperty(string property)
        {
            return GetMetadata(property)?.Value;
        }

        public void SetProperty(string property, string value)
        {
            SetMetadata(property, value);
        }

        public void SetProperty(string property, string value, string condition)
        {
            SetMetadata(property, value, condition);
        }

        public MetadataCollection Properties => _propertyCollection;
    }
}