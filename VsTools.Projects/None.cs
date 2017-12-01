using System.Xml.Linq;

namespace VsTools.Projects
{
    public class None : ItemGroupContent
    {
        public None(XNode node) : base(node)
        {

        }

        public None(string include) : base(include)
        {

        }
    }
}