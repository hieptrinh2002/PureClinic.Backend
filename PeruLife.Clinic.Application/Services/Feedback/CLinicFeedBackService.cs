using AutoMapper;
using PureLifeClinic.Application.BusinessObjects.Feedbacks.Clinic;
using PureLifeClinic.Application.Interfaces.IServices.FeedBack;
using PureLifeClinic.Core.Entities.General.Feedback;
using PureLifeClinic.Core.Interfaces.IRepositories;

namespace PureLifeClinic.Application.Services.FeedBack
{
    public class CLinicFeedBackService : BaseService<ClinicFeedBack, ClinicFeedBackViewModel>, IClinicFeedbackService   
    {
        private readonly IBaseRepository<ClinicFeedBack> _repository;
        private readonly IMapper _mapper;
        public CLinicFeedBackService(IMapper mapper, IBaseRepository<ClinicFeedBack> repository) : base(mapper, repository)
        {
            _repository = repository;
            _mapper = mapper;
        }
    }
}
