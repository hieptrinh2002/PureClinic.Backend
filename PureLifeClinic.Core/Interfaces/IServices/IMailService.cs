using AutoMapper.Internal;
using PureLifeClinic.Core.Entities.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequestViewModel mailRequest);
    }
}
