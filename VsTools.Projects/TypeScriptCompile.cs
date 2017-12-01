namespace VsTools.Projects
{
    public class TypeScriptCompile : ItemGroupFileContent
    {
        public TypeScriptCompile(string include) : base(include)
        {

        }
        public TypeScriptCompile(string include, string dependentUpon) : base(include, dependentUpon)
        {

        }
        public TypeScriptCompile(string include, string dependentUpon, string subType) : base(include, dependentUpon, subType)
        {

        }
    }
}