using System.Xml.Linq;

namespace VsTools.Projects
{
    public class ProjectReference : ItemGroupContent
    {
        private string _project;
        private string _name;

        public string Project
        {
            get { return _project; }
            set
            {
                AddOrUpdateElement("Project", value);
                _project = value;
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                AddOrUpdateElement("Name", value);
                _name = value;
            }
        }

        public ProjectReference(XNode node) : base(node)
        {
            var els = GetXElements();
            _project = els.TryGetXElementValue("Value");
            _name = els.TryGetXElementValue("Name");
        }

        public ProjectReference(string include, string project, string name) : base(include)
        {
            Project = project;
            Name = name;
        }
    }
}