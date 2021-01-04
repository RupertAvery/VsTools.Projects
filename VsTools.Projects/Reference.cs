using System.Xml.Linq;

namespace VsTools.Projects
{
    public class Reference : ItemGroupContent
    {
        private string _hintPath;
        private string _private;
        private string _specificVersion;

        public string HintPath
        {
            get { return _hintPath; }
            set
            {
                AddOrUpdateElement("HintPath", value);
                _hintPath = value;
            }
        }

        public string Private
        {
            get { return _private; }
            set
            {
                AddOrUpdateElement("Private", value);
                _private = value;
            }
        }

        public string SpecificVersion
        {
            get { return _specificVersion; }
            set
            {
                AddOrUpdateElement("SpecificVersion", value);
                _specificVersion = value;
            }
        }

        public Reference(XNode node) : base(node)
        {
            var els = GetXElements();
            _hintPath = els.TryGetXElementValue("HintPath");
            _private = els.TryGetXElementValue("Private");
            _specificVersion = els.TryGetXElementValue("SpecificVersion");
        }

        public Reference(string include) : base(include)
        {
        }

        public Reference(string include, string hintPath) : base(include)
        {
            HintPath = hintPath;
        }

    }
}