using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace VsTools.Projects
{
    /// <summary>
    /// Represents an untyped ItemGroup child element
    /// </summary>
    public class Item : ProjectElement
    {
        private MetadataCollection _metadata;

        public override int Depth => 2;

        public string Include
        {
            get => GetAttributeValue();
            set => AddOrUpdateAttribute(value);
        }

        public string Exclude
        {
            get => GetAttributeValue();
            set => AddOrUpdateAttribute(value);
        }

        protected Item()
        {
            _metadata = new MetadataCollection(this);
        }

        protected Item(XNode node) : base(node)
        {
            _metadata = new MetadataCollection(this);
        }

        protected Item(string include)
        {
            _metadata = new MetadataCollection(this);
            Include = include;
        }

        public Item(string name, string include) : base(name)
        {
            _metadata = new MetadataCollection(this);
            Include = include;
        }

        //https://docs.microsoft.com/en-us/visualstudio/msbuild/item-element-msbuild?view=vs-2019#specify-metadata-as-attributes

        public string GetMetadataValue([CallerMemberName] string name = null)
        {
            var element = GetElement(name);
            if (element != null)
            {
                return element.Value;
            }
            return GetAttributeValue(name);
        }

        public void AddOrUpdateMetadataValue(string value, [CallerMemberName] string name = null)
        {
            //AddOrUpdateAttribute(value, name);
            AddOrUpdateElement(name, value);
        }

        public MetadataCollection Metadata => _metadata;


    }


}