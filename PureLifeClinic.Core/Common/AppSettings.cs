
using PureLifeClinic.Core.Common;

namespace Project.Core.Common
{
    public class AppSettings
    {
        public JwtConfig? JwtConfig { get; set; }
        public MailSettings? MailSettings { get; set; }
    }
}
