using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureLifeClinic.Core.Services.BackgroundJob.RabbitMQ.Consumers
{
    public interface IMessageConsumer
    {
        Task ConsumeAsync();
    }

}
