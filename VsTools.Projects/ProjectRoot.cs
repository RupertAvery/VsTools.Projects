using System.Xml.Linq;

namespace VsTools.Projects
{
    public class ProjectRoot : ProjectElement
    {
        public string Sdk
        {
            get => GetAttributeValue();
            set => AddOrUpdateAttribute(value);
        }

        public string ToolsVersion
        {
            get => GetAttributeValue();
            set => AddOrUpdateAttribute(value);
        }

        public override int Depth => 0;

        public ProjectRoot()
        {

        }

        public ProjectRoot(XNode node) : base(node)
        {

        }

        public ProjectRoot(bool is2017Project, string toolsVersion = "15.0") : base(CreateProject(is2017Project, toolsVersion))
        {
        }


        private static XElement CreateProject(bool is2017Project, string toolsVersion)
        {
            if (is2017Project)
            {
                return CreateVS2017Project();
            }
            else
            {
                return CreatePreVS2017Project(toolsVersion);
            }
        }

        private static XElement CreatePreVS2017Project(string toolsVersion = "15.0")
        {
            var projectElement = new XElement(Project.MsbuildSchema + "Project");
            projectElement.SetAttributeValue("ToolsVersion", toolsVersion);
            return projectElement;
        }

        private static XElement CreateVS2017Project()
        {
            var projectElement = new XElement("Project");
            projectElement.SetAttributeValue("Sdk", "Microsoft.NET.Sdk");
            return projectElement;
        }
    }
}