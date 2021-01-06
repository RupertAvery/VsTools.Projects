using System.Linq;
using System.Xml.Linq;

namespace VsTools.Projects
{
    public class Target : ProjectElement
    {
        public override int Depth => 2;

        public string Name
        {
            get => GetAttributeValue();
            set => AddOrUpdateAttribute(value);
        }

        public string BeforeTargets
        {
            get => GetAttributeValue();
            set => AddOrUpdateAttribute(value);
        }

        public string AfterTargets
        {
            get => GetAttributeValue();
            set => AddOrUpdateAttribute(value);
        }

        public Target(XNode node) : base(node)
        {
        }
    }
}