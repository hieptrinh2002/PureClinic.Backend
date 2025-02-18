namespace PureLifeClinic.Core.Entities.General
{
    public class Permission: Base<int>    
    {
        public string Name { get; private set; }

        public Permission(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
