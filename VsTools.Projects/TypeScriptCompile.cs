namespace VsTools.Projects
{
    public class TypeScriptCompile : Item
    {
        public string SubType
        {
            get => GetMetadataValue();
            set => AddOrUpdateMetadataValue(value);
        }

        public string DependentUpon
        {
            get => GetMetadataValue();
            set => AddOrUpdateMetadataValue(value);
        }

        public TypeScriptCompile(string include) : base(include)
        {

        }
    }
}