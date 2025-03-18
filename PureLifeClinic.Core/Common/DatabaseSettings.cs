using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureLifeClinic.Core.Common
{
    public class DatabaseSettings
    {
        public string PrimaryDbConnection { get; set; } = string.Empty;
        public string HangfireDbConnection { get; set; } = string.Empty;
        public string RedisConnection { get; set; } = string.Empty;
    }

}
