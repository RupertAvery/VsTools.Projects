using System.Xml.Linq;

namespace VsTools.Projects
{
    /// <summary>
    /// A wrapper around <see cref="XAttribute"/>
    /// </summary>
    public class Attribute
    {
        private readonly XAttribute _attribute;

        public string Value
        {
            get { return _attribute.Value; }
            set { _attribute.Value = value; }
        }

        public Attribute(XAttribute attribute)
        {
            this._attribute = attribute;
        }

        public Attribute(string name, string value)
        {
            this._attribute = new XAttribute(XName.Get(name), value);
        }
    }
}