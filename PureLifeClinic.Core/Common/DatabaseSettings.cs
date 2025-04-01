namespace PureLifeClinic.Core.Common
{
    public class DatabaseSettings
    {
        public string PrimaryDbConnection { get; set; } = string.Empty;
        public string HangfireDbConnection { get; set; } = string.Empty;
        public string RedisConnection { get; set; } = string.Empty;
    }

}
