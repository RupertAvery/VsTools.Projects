using System.Xml.Linq;

namespace VsTools.Projects
{
    public class Compile : ItemGroupFileContent
    {
        public Compile() 
        {

        }

        public Compile(XNode node) : base(node)
        {
            
        }

        public Compile(string include) : base(include)
        {
            
        }

        public Compile(string include, string dependentUpon) : base(include, dependentUpon)
        {

        }

        public Compile(string include, string dependentUpon, string subType) : base(include, dependentUpon, subType)
        {

        }
    }
}