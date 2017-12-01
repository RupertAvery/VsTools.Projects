using System.Xml.Linq;

namespace VsTools.Projects
{
    public abstract class ItemGroupFileContent : ItemGroupContent
    {

        private string _dependentUpon;
        private string _subtype;

        public string DependentUpon
        {
            get { return _dependentUpon; }
            set
            {
                AddOrUpdateElement("DependentUpon", value);
                _dependentUpon = value;
            }
        }

        public string SubType
        {
            get { return _subtype; }
            set
            {
                AddOrUpdateElement("SubType", value);
                _subtype = value;
            }
        }

        protected ItemGroupFileContent()
        {

        }

        protected ItemGroupFileContent(XNode node) : base(node)
        {
            var els = GetXElements();
            _dependentUpon = els.TryGetXElementValue("DependentUpon");
            _subtype = els.TryGetXElementValue("SubType");
        }

        protected ItemGroupFileContent(string include) : base(include)
        {
        }

        protected ItemGroupFileContent(string include, string dependentUpon) : base(include)
        {
            DependentUpon = dependentUpon;
        }

        protected ItemGroupFileContent(string include, string dependentUpon, string subType) : base(include)
        {
            DependentUpon = dependentUpon;
            SubType = subType;
        }

    }
}