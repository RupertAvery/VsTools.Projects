using System.Xml.Linq;

namespace VsTools.Projects
{
    public class ItemGroupContent : CsProjectNode
    {
        public override int Depth => 2;

        private string _include;

        public override string ElementName {
            get { return ((XElement) Node).Name.LocalName; }
        }

        public string Include
        {
            get { return _include; }
            set
            {
                AddOrUpdateAttribute("Include", value);
                _include = value;
            }
        }

        protected ItemGroupContent()
        {

        }

        protected ItemGroupContent(XNode node) : base(node)
        {
            _include = Element.Attribute("Include").Value;
        }

        protected ItemGroupContent(string include)
        {
            Include = include;
        }
    }
}