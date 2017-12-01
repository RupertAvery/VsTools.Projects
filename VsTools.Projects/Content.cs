using System.Xml.Linq;

namespace VsTools.Projects
{
    public class Content : ItemGroupFileContent
    {
        public Content(XNode node) : base(node)
        {

        }

        public Content(string include) : base(include)
        {

        }

        public Content(string include, string dependentUpon) : base(include, dependentUpon)
        {

        }

        public Content(string include, string dependentUpon, string subType) : base(include, dependentUpon, subType)
        {

        }
    }
}