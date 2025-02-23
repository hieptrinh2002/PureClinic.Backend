namespace PureLifeClinic.Core.Common
{
    public class AppSettings
    {
        public JwtConfig? JwtConfig { get; set; }
        public MailSettings? MailSettings { get; set; }
        public CloudinaryConfig? CloudinaryConfig { get; set; }

        public ClinicInfo? ClinicInfo { get; set; } 
    }
}
