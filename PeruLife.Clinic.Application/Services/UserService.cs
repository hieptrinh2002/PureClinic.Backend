using AutoMapper;
using PureLifeClinic.Application.BusinessObjects.AuthViewModels.ForgotPassword;
using PureLifeClinic.Application.BusinessObjects.AuthViewModels.ResetPassword;
using PureLifeClinic.Application.BusinessObjects.EmailViewModels;
using PureLifeClinic.Application.BusinessObjects.ErrorViewModels;
using PureLifeClinic.Application.BusinessObjects.PatientsViewModels;
using PureLifeClinic.Application.BusinessObjects.ResponseViewModels;
using PureLifeClinic.Application.BusinessObjects.UserViewModels;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IRepositories;
using System.Linq.Expressions;
using PureLifeClinic.Core.Common.Constants;
using PureLifeClinic.Application.Extentions.Mapping;
namespace PureLifeClinic.Application.Services
{
    public class UserService : BaseService<User, UserViewModel>, IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;
        private readonly IEmailTemplateService _emailTemplateService;

        public UserService(
            IMapper mapper, 
            IUserContext userContext, 
            IUnitOfWork unitOfWork,
            IEmailTemplateService emailTemplateService) : base(mapper, unitOfWork.Users)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userContext = userContext;
            _emailTemplateService = emailTemplateService;
        }

        public new async Task<IEnumerable<UserViewModel>> GetAll(CancellationToken cancellationToken)
        {
            var includeList = new List<Expression<Func<User, object>>> { x => x.Role };
            var entities = await _unitOfWork.Users.GetAll(includeList, cancellationToken);
            return UserMappingExts.MapToListUserViewModel(entities.ToList());
        }

        public async Task<IEnumerable<PatientViewModel>> GetAllPatient(CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.Users.GetAllPatient(cancellationToken);
            return _mapper.Map<IEnumerable<PatientViewModel>>(entities);
        }

        public new async Task<PaginatedData<UserViewModel>> GetPaginatedData(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var includeList = new List<Expression<Func<User, object>>> { x => x.Role, x => x.Patient, x => x.Doctor };

            var paginatedData = await _unitOfWork.Users.GetPaginatedData(includeList, pageNumber, pageSize, cancellationToken);
            var mappedData = UserMappingExts.MapToListUserViewModel(paginatedData.Data.ToList());
            var paginatedDataViewModel = new PaginatedData<UserViewModel>(mappedData.ToList(), paginatedData.TotalCount);
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

            if (await IsExists("UserName", model.UserName, cancellationToken))
            {
                throw new BadRequestException($"The user name- '{model.UserName}' already exists", ErrorCode.DuplicateUserNameError);
            }

            if (await IsExists("Email", model.Email, cancellationToken))
            {
                var message = $"The user Email- '{model.Email}' already exists";
                throw new BadRequestException(message, ErrorCode.DuplicateUserNameError);
            }

            var user = new User
            {
                FullName = model.FullName,
                UserName = model.UserName,
                Email = model.Email,
                IsActive = true,
                RoleId = model.RoleId,
                EntryDate = DateTime.Now,
                EntryBy = Convert.ToInt32(_userContext.UserId)
            };

            var result = await _unitOfWork.Users.Create(user, model.Password);
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
            var user = await _unitOfWork.Users.GetById(model.Id, cancellationToken) ?? throw new NotFoundException("User not found");
            user.FullName = model.FullName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.RoleId = model.RoleId;
            user.UpdatedDate = DateTime.Now;
            user.UpdatedBy = Convert.ToInt32(_userContext.UserId);

            var result = await _unitOfWork.Users.Update(user);
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
            var (UserId, Token) = await _unitOfWork.Users.GenerateEmailConfirmationTokenAsync(email);

            return new ResponseViewModel<EmailActivationViewModel>
            {
                Success = true,
                Data = new EmailActivationViewModel
                {
                    UserId = UserId,
                    ActivationToken = Token,
                }
            };
        }

        public async Task<bool> UnlockAccountAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetById(userId, default);
            return user == null ? throw new NotFoundException("User not found.") : await _unitOfWork.Users.UnlockAccountAsync(user);
        }

        public async Task<ResponseViewModel> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _unitOfWork.Users.GetByEmail(email, default) 
                ?? throw new NotFoundException("User not found.");

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

            var dictVariables = new Dictionary<string, string>
            {
                {"{{UserName}}", user.UserName },
                {"{{ResetPasswordLink}}", resetLink },
                {"{{Year}}", DateTime.Now.Year.ToString() },
                {"{{UserEmail}}", user.Email }
            };

            var emailBody = await _emailTemplateService.RenderTemplateAsync("ForgotPassword.html", dictVariables);

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
