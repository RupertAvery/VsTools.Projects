using System.Xml.Linq;

namespace VsTools.Projects
{
    public class Import : CsProjectNode
    {
        private string _project;
        public override int Depth => 1;

        public string Project
        {
            get { return _project; }
            set
            {
                AddOrUpdateAttribute("Project", value);
                _project = value;
            }
        }

        public Import(string project)
        {
            Project = project;
        }

        public Import(XNode node): base(node)
        {
            _project = Element.Attribute("Project").Value;
        }

    }
}