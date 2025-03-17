namespace PureLifeClinic.Core.Entities.Interfaces
{
    public interface IHasOwner<T>
    {
        T OwnerId { set; get; }
    }
}
