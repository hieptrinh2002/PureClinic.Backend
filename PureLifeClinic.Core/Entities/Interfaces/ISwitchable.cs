using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Core.Entities.Interfaces
{
    public interface ISwitchable
    {
        Status Status { set; get; }
    }
}
