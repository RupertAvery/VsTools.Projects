using System.Xml.Linq;

namespace VsTools.Projects
{
    public class Compile : Item
    {
        public string DependentUpon
        {
            get => GetMetadataValue();
            set => AddOrUpdateMetadataValue(value);
        }

        public string SubType
        {
            get => GetMetadataValue();
            set => AddOrUpdateMetadataValue(value);
        }

        public string CopyToOutputDirectory
        {
            get => GetMetadataValue();
            set => AddOrUpdateMetadataValue(value);
        }

        public Compile() 
        {

        }

        public Compile(XNode node) : base(node)
        {
            
        }

        public Compile(string include) : base(include)
        {
            
        }
    }
}