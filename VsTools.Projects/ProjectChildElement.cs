using System.Xml.Linq;

namespace VsTools.Projects
{
    public abstract class ProjectChildElement : ProjectElement
    {
        public override int Depth => 1;
        
        protected ProjectChildElement() 
        {

        }

        protected ProjectChildElement(string name) : base(name)
        {

        }
        protected ProjectChildElement(XNode node) : base(node)
        {

        }
    }
}