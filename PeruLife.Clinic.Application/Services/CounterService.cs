using AutoMapper;
using PureLifeClinic.Application.BusinessObjects.CounterViewModels.Response;
using PureLifeClinic.Application.Interfaces;
using PureLifeClinic.Core.Entities.General.Queues;
using PureLifeClinic.Core.Interfaces.IRepositories;

namespace PureLifeClinic.Application.Services
{
    public class CounterService : BaseService<Counter, CounterViewModel>, ICounterService
    {
        public CounterService(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork.Counters)
        {
        }
    }
}
