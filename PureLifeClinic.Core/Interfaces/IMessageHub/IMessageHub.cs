namespace PureLifeClinic.Core.Interfaces.IMessageHub
{
    public interface IMessageHub
    {
        Task SendNotification(List<string> message);
    }
}
