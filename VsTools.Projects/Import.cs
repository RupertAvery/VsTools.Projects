using System.Xml.Linq;

namespace VsTools.Projects
{
    public class Import : ProjectChildElement
    {
        public string Project
        {
            get => GetAttributeValue();
            set => AddOrUpdateAttribute(value);
        }

        public Import(string project)
        {
            Project = project;
        }

        public Import(XNode node): base(node)
        {
        }
    }
}