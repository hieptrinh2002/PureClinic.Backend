
using PureLifeClinic.Core.Common;

namespace PureLifeClinic.Core.Common
{
    public class AppSettings
    {
        public JwtConfig? JwtConfig { get; set; }
        public MailSettings? MailSettings { get; set; }
    }
}
