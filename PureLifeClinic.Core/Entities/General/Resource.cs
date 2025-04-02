namespace PureLifeClinic.Core.Entities.General
{
    public class Resource : Base<int>
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string ParentId { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }
    }
}
