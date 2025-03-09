using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.Core.Services
{

    public class MedicineService : BaseService<Medicine, MedicineViewModel>, IMedicineService
    {
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;
        public MedicineService(
            IMapper mapper,
            IUserContext userContext,
            IUnitOfWork unitOfWork)
            : base(mapper, unitOfWork.Medicines)
        {
            _mapper = mapper;
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<MedicineViewModel> Create(MedicineCreateViewModel model, CancellationToken cancellationToken)
        {
            //Mapping through AutoMapper
            var entity = _mapper.Map<Medicine>(model);
            entity.EntryDate = DateTime.Now;
            entity.EntryBy = Convert.ToInt32(_userContext.UserId);

            try
            {
                var result = await _unitOfWork.Medicines.Create(entity, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return _mapper.Map<MedicineViewModel>(result);
            }
            catch (DbUpdateException ex)
            {
                throw new ErrorException($"An error occurred while creating the medicine {entity.Name} - {ex.Message}");
            }
        }

        public async Task Update(MedicineUpdateViewModel model, CancellationToken cancellationToken)
        {
            var existingData = await _unitOfWork.Medicines.GetById(model.Id, cancellationToken)
                ?? throw new KeyNotFoundException($"Medicine with ID {model.Id} not found.");

            _mapper.Map(model, existingData);
            existingData.UpdatedDate = DateTime.Now;
            existingData.UpdatedBy = Convert.ToInt32(_userContext.UserId);
            try
            {
                await _unitOfWork.Medicines.Update(existingData, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                throw new ErrorException($"An error occurred while updating the medicine id {model.Id} - {ex.Message}.");
            }
        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Medicines.GetById(id, cancellationToken)
                ?? throw new KeyNotFoundException($"Medicine with ID {id} not found.");
            try
            {
                await _unitOfWork.Medicines.Delete(entity, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new ErrorException($"An error occurred while deleting the medicine with id - {ex.Message}.");
            }
        }

        public async Task<double> PriceCheck(int medicineId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Medicines.PriceCheck(medicineId, cancellationToken);
        }
    }
}
