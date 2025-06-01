using PureLifeClinic.Application.BusinessObjects.PatientsViewModels;
using PureLifeClinic.Application.Extentions.Mapping;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IRepositories;
using System.Linq.Expressions;

namespace PureLifeClinic.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PatientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Patient> CreateAsync(PatientCreateViewModel model, CancellationToken cancellationToken)
        {
            // check patient already exists
            var patientExists = _unitOfWork.Patients.GetByUserId(model.UserId, cancellationToken)
                 ?? throw new BadRequestException($"Patient with user id - {model.UserId} already exists !");

            var patient = model.MapToPatient();
            return await _unitOfWork.Patients.Create(patient, cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var patient = await _unitOfWork.Patients.GetById(id, cancellationToken);
            if (patient == null) return false;

            patient.IsDeleted = true;
            await _unitOfWork.Patients.Update(patient, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<PatientViewModel> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var includeList = new List<Expression<Func<Patient, object>>> { x => x.User };

            Patient patient = await _unitOfWork.Patients.GetById(includeList, id, cancellationToken)
                ?? throw new NotFoundException($"Patient with id - {id} is not found !");

            return patient.ToPatientViewModel();
        } 

        public async Task<bool> UpdateAsync(int id, PatientUpdateViewModel model, CancellationToken cancellationToken)
        {
            var patient = await _unitOfWork.Patients.GetById(id, cancellationToken)
                ?? throw new NotFoundException("Patient not found.");

            patient.Notes = model.Notes;
            patient.PrimaryDoctorId = model.PrimaryDoctorId;
            patient.RequireDeposit = model.RequireDeposit;
            // cập nhật các trường khác nếu cần

            await _unitOfWork.Patients.Update(patient, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
