namespace PureLifeClinic.Core.Entities.Interfaces
{
    public interface IHasSoftDelete
    {
        bool IsDeleted { set; get; }

        DateTime? DeleteAt { set; get; }
    }
}
