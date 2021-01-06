using System.Xml.Linq;

namespace VsTools.Projects
{
    public class Element : ProjectElement
    {
        private string _value;
        private readonly string _elementName;
        public override int Depth { get; }

        public override string ElementName => _elementName;

        public string Value
        {
            get { return _value; }
            set
            {
                Element.Value = value;
                _value = value;
            }
        }

        public Element(string name, string value, int depth)
        {
            _elementName = name;
            Value = value;
            Depth = depth;
        }

        public Element(XNode node, int depth) : base(node)
        {
            _elementName = Element.Name.LocalName;
            _value = Element.Value;
            Depth = depth;
        }

    }
}