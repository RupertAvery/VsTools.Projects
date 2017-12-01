using System.Linq;
using System.Xml.Linq;

namespace VsTools.Projects
{
    public class Target : CsProjectNode
    {
        private string _name;
        private string _beforeTargets;
        private string _afterTargets;
        public override int Depth => 2;

        public string Name
        {
            get { return _name; }
            set
            {
                AddOrUpdateAttribute("Name", value);
                _name = value;
            }
        }

        public string BeforeTargets
        {
            get { return _beforeTargets; }
            set
            {
                AddOrUpdateAttribute("BeforeTargets", value);
                _beforeTargets = value;
            }
        }

        public string AfterTargets
        {
            get { return _afterTargets; }
            set
            {
                AddOrUpdateAttribute("AfterTargets", value);
                _afterTargets = value;
            }
        }

        public Target(XNode node) : base(node)
        {
            _name = Element.Attributes().First(y => y.Name.LocalName == "Name").Value;
            _afterTargets = Element.Attributes().FirstOrDefault(y => y.Name.LocalName == "AfterTargets")?.Value;
            _beforeTargets = Element.Attributes().FirstOrDefault(y => y.Name.LocalName == "BeforeTargets")?.Value;
        }
    }
}