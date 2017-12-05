using System.Xml.Linq;

namespace VsTools.Projects
{
    public class EmbeddedResource : ItemGroupContent
    {
        private string _logicalName;

        public string LogicalName
        {
            get { return _logicalName; }
            set
            {
                AddOrUpdateElement("LogicalName", value);
                _logicalName = value;
            }
        }
        public EmbeddedResource()
        {
        }

        public EmbeddedResource(XNode node) : base(node)
        {
            var els = GetXElements();
            _logicalName = els.TryGetXElementValue("HintPath");
        }

        public EmbeddedResource(string include, string logicalName) : base(include)
        {
            LogicalName = logicalName;
        }


    }
}