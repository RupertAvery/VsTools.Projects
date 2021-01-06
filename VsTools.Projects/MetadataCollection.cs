namespace VsTools.Projects
{
    public class MetadataCollection
    {
        private readonly ProjectElement _parent;

        public MetadataCollection(ProjectElement parent)
        {
            _parent = parent;
        }

        public Property this[string index]
        {
            get => _parent.GetMetadata(index);
            set => _parent.SetMetadata(value);
        }
    }
}