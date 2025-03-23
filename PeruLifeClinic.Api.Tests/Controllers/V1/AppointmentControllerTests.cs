using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PureLifeClinic.API.Controllers.V1;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Enums;
using PureLifeClinic.Core.Interfaces.IServices;
using Xunit;

namespace PeruLifeClinic.Api.Tests.Controllers.V1
{
    public class AppointmentControllerTests
    {
        private readonly Mock<IAppointmentService> _mockAppointmentService;
        private readonly Mock<ILogger<AppointmentController>> _mockLogger;
        private readonly AppointmentController _controller;
        public AppointmentControllerTests()
        {
            _mockAppointmentService = new Mock<IAppointmentService>();
            _mockLogger = new Mock<ILogger<AppointmentController>>();
            _controller = new AppointmentController(_mockAppointmentService.Object, _mockLogger.Object);
        }

        #region [HttpGet] GetAppointments

        [Fact]
        public async Task GetAppointments_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var appointments = new List<AppointmentViewModel> { new AppointmentViewModel() };
            _mockAppointmentService.Setup(s => s.GetAll(It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments);

            // Act
            var result = await _controller.Get(It.IsAny<CancellationToken>());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseViewModel<IEnumerable<AppointmentViewModel>>>(okResult.Value);
            Assert.True(response.Success);
        }

        #endregion

        #region [HttpGet(Paginated-data)] GetbyFilterCondition

        [Fact]
        public async Task GetbyFilterCondition_WhenCalled_ReturnsOkResult()
        {
            //Arrange
            var paginatedData = new PaginatedDataViewModel<AppointmentViewModel>(
                new List<AppointmentViewModel> {
                    new AppointmentViewModel()
                }, 1);
            _mockAppointmentService.Setup(s => s.GetPaginatedData(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<List<ExpressionFilter>>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(paginatedData);

            var result = await _controller.GetbyFilterCondition(null, null, null, null, null, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseViewModel<PaginatedDataViewModel<AppointmentViewModel>>>(okResult.Value);
            Assert.True(response.Success);
        }
        #endregion


        [Fact]
        public async Task GetFilterAppointment_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var filterResult = new List<AppointmentViewModel> { new AppointmentViewModel() };
            var responseViewModel = new ResponseViewModel<IEnumerable<AppointmentViewModel>>
            {
                Data = filterResult,
                Success = true
            };
            _mockAppointmentService.Setup(s => s.GetAllFilterAppointments(
                It.IsAny<FilterAppointmentRequestViewModel>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(responseViewModel);

            // Act
            var result = await _controller.GetFilterAppointment(new FilterAppointmentRequestViewModel(), CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseViewModel<IEnumerable<AppointmentViewModel>>>(okResult.Value);
            Assert.Equal(responseViewModel, okResult.Value);
        }

        [Fact]
        public async Task GetAppointmentsByDoctor_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var appointments = new List<DoctorAppointmentViewModel> { new DoctorAppointmentViewModel() };
            var responseViewModel = new ResponseViewModel<IEnumerable<DoctorAppointmentViewModel>>
            {
                Data = appointments,
                Success = true
            };

            _mockAppointmentService.Setup(s => s.GetAllAppointmentsByDoctorIdAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>())
            ).ReturnsAsync(responseViewModel);

            // Act
            var result = await _controller.GetAppointmentsByDoctor(1, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseViewModel<IEnumerable<DoctorAppointmentViewModel>>>(okResult.Value);
            Assert.Equal(responseViewModel, response);
        }

        [Fact]
        public async Task GetAppointmentsByPatient_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var appointments = new List<PatientAppointmentViewModel> { new PatientAppointmentViewModel() };
            var responseViewModel = new ResponseViewModel<IEnumerable<PatientAppointmentViewModel>>
            {
                Data = appointments,
                Success = true
            };

            _mockAppointmentService.Setup(s => s.GetAllAppointmentsByPatientIdAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>())
            ).ReturnsAsync(responseViewModel);

            // Act
            var result = await _controller.GetAppointmentsByPatient(1, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseViewModel<IEnumerable<PatientAppointmentViewModel>>>(okResult.Value);
            Assert.Equal(responseViewModel, response);
        }

        [Fact]
        public async Task Create_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var appointment = new AppointmentViewModel();
            _mockAppointmentService.Setup(s => s.Create(
                It.IsAny<AppointmentCreateViewModel>(), It.IsAny<CancellationToken>())
            ).ReturnsAsync(appointment);

            // Act
            var result = await _controller.Create(new AppointmentCreateViewModel(), CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseViewModel<AppointmentViewModel>>(okResult.Value);
            Assert.True(response.Success);
        }

        [Fact]
        public async Task CreateInPersonAppointment_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var appointment = new AppointmentViewModel();
            _mockAppointmentService.Setup(s => s.CreateInPersonAppointment(
                It.IsAny<InPersonAppointmentCreateViewModel>(), It.IsAny<CancellationToken>())
            ).ReturnsAsync(appointment);

            _mockAppointmentService.Setup(s => s.IsExists(
                It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())
            ).ReturnsAsync(false);

            // Act
            var result = await _controller.CreateInPersonAppointment(new InPersonAppointmentCreateViewModel(), CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseViewModel<AppointmentViewModel>>(okResult.Value);
            Assert.True(response.Success);
        }

        [Fact]
        public async Task UpdateAppointment_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var updateResult = new ResponseViewModel { Success = true };
            _mockAppointmentService.Setup(s => s.UpdateAppointmentAsync(
                It.IsAny<int>(), It.IsAny<AppointmentUpdateViewModel>(), It.IsAny<CancellationToken>())
            ).ReturnsAsync(updateResult);

            // Act
            var result = await _controller.UpdateAppointment(1, new AppointmentUpdateViewModel(), CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseViewModel>(okResult.Value);
            Assert.True(response.Success);
        }

        [Fact]
        public async Task UpdateAppointmentStatus_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var updateResult = new ResponseViewModel { Success = true };
            _mockAppointmentService.Setup(s => s.UpdateAppointmentStatusAsync(
                It.IsAny<int>(), It.IsAny<AppointmentStatus>(), It.IsAny<CancellationToken>())
            ).ReturnsAsync(updateResult);

            // Act
            var result = await _controller.UpdateAppointmentStatus(
                new AppointmentStatusUpdateViewModel(), (int)AppointmentStatus.Confirmed, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseViewModel>(okResult.Value);
            Assert.True(response.Success);
        }
    }
}
