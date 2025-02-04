using AutoMapper;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;
using System.Linq.Expressions;

namespace PureLifeClinic.Core.Services
{
    public class UserService : BaseService<User, UserViewModel>, IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork.Users)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public new async Task<IEnumerable<UserViewModel>> GetAll(CancellationToken cancellationToken)
        {
            var includeList = new List<Expression<Func<User, object>>> { x => x.Role };
            var entities = await _unitOfWork.Users.GetAll(includeList, cancellationToken);

            return _mapper.Map<IEnumerable<UserViewModel>>(entities);
        }
       
        public async Task<IEnumerable<PatientViewModel>> GetAllPatient(CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.Users.GetAllPatient(cancellationToken);
            return _mapper.Map<IEnumerable<PatientViewModel>>(entities);
        }

        public new async Task<PaginatedDataViewModel<UserViewModel>> GetPaginatedData(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var includeList = new List<Expression<Func<User, object>>> { x => x.Role, x=> x.Patient, x => x.Doctor  };

            var paginatedData = await _unitOfWork.Users.GetPaginatedData(includeList, pageNumber, pageSize, cancellationToken);
            var mappedData = _mapper.Map<IEnumerable<UserViewModel>>(paginatedData.Data);
            var paginatedDataViewModel = new PaginatedDataViewModel<UserViewModel>(mappedData.ToList(), paginatedData.TotalCount);
            return paginatedDataViewModel;
        }

        public async Task<UserViewModel> GetById(int id, CancellationToken cancellationToken)
        {
            var includeList = new List<Expression<Func<User, object>>> { x => x.Role };

            return _mapper.Map<UserViewModel>(await _unitOfWork.Users.GetById(includeList, id, cancellationToken));
        }

        public Task<User> GetByEmail(string email, CancellationToken cancellationToken)
        {
            var user = _unitOfWork.Users.GetByEmail(email, cancellationToken);   
            return user;    
        }

        public async Task<ResponseViewModel> Create(UserCreateViewModel model, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Users.Create(model);
            if (result.Succeeded)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return new ResponseViewModel { Success = true, Message = "User created successfully" };
            }
            else
            {
                return new ResponseViewModel
                {
                    Success = false,
                    Message = "User creation failed",
                    Error = new ErrorViewModel
                    {
                        Code = "USER_CREATION_ERROR",
                        Message = string.Join(", ", result.Errors.Select(e => e.Description))
                    }
                };
            }
        }

        public async Task<ResponseViewModel> Update(UserUpdateViewModel model, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Users.Update(model);
            if (result.Succeeded)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return new ResponseViewModel { Success = true, Message = "User updated successfully" };
            }
            else
            {
                return new ResponseViewModel
                {
                    Success = false,
                    Message = "User update failed",
                    Error = new ErrorViewModel
                    {
                        Code = "USER_UPDATE_ERROR",
                        Message = string.Join(", ", result.Errors.Select(e => e.Description))
                    }
                };
            }
        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Users.GetById(id, cancellationToken);
            await _unitOfWork.Users.Delete(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken); 
        }

        public async Task<ResponseViewModel<EmailActivationViewModel>> GenerateEmailConfirmationTokenAsync(string email)
        {
            var result =  await _unitOfWork.Users.GenerateEmailConfirmationTokenAsync(email);
            if (result == null)
            {
                return new ResponseViewModel<EmailActivationViewModel>
                {
                    Success = false,
                    Message= "User not found !",
                    Data = null,
                };
            }
            return new ResponseViewModel<EmailActivationViewModel>
            {
                Success = true,
                Data = new EmailActivationViewModel
                {
                    UserId = result.UserId,
                    ActivationToken = result.ActivationToken
                }
            };
        }

        public async Task<bool> UnlockAccountAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetById(userId, default);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }
            return await _unitOfWork.Users.UnlockAccountAsync(user);
        }

        public async Task<ResponseViewModel> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _unitOfWork.Users.GetByEmail(email, default);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            var result = await _unitOfWork.Users.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
            {
                await _unitOfWork.SaveChangesAsync();
                return new ResponseViewModel { Success = true, Message = "Password reset successfully" };
            }
            return new ResponseViewModel { Success = false, Message = "Password reset failed" };
        }

        public async Task<ResponseViewModel<ResetPasswordViewModel>> GenerateResetPasswordTokenAsync(ForgotPasswordRequestViewModel model)
        {
            var user = await _unitOfWork.Users.GetByEmail(model.Email, default);
            if (user == null) 
                return new ResponseViewModel<ResetPasswordViewModel>
                {
                    Success = false,
                    Message = "genarate reset password token failed"
                };

            var resetToken = await _unitOfWork.Users.GenerateResetPasswordTokenAsync(user);
            var _token = Uri.EscapeDataString(resetToken);
            var email = Uri.EscapeDataString(model.Email);
            var resetLink = $"{model.ClientUrl}?token={_token}&email={email}";

            // get mail body
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Template", "ForgotPassword.html");
            var emailBody = System.IO.File.ReadAllText(filePath);
            emailBody = emailBody
                .Replace("{{UserName}}", user.UserName)
                .Replace("{{ResetPasswordLink}}", resetLink)
                .Replace("{{Year}}", DateTime.Now.Year.ToString())
                .Replace("{{UserEmail}}", user.Email);

            return new ResponseViewModel<ResetPasswordViewModel>
            {
                Success = true,
                Message = "Genarate reset password token successfully",
                Data = new ResetPasswordViewModel
                {
                    Token = resetToken,
                    Url = resetLink,
                    EmailBody = emailBody   
                }
            };
        }
    }
}
