using System.Xml.Linq;

namespace VsTools.Projects
{
    public class Folder : Item
    {
        public Folder()
        {

        }

        public Folder(XNode node) : base(node)
        {

        }

        public Folder(string include) : base(include)
        {

        }
    }
}