using App.Onion.Domain.Interfaces.PatientRepository;
using AutoMapper;
using dddnet8.AuditLog.Interfaces;
using dddnet8.Domain.OperationRequests;
using dddnet8.Domain.PlanningModuleNotifications;
using dddnet8.Domain.Shared;
using dddnet8.Domain.Staffs;
using dddnet8.Domain.Staffs.Interfaces;
using dddnet8.Domain.SystemUsers;
using dddnet8.Infraestructure.OperationRequests;
using dddnet8.Infraestructure.OperationTypes;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace dddnet8.Tests.Domain.OperationRequests
{
    [TestFixture]
    public class OperationRequestServiceTests
    {
        private Mock<IOperationRequestPolicy> _operationRequestPolicyMock;
        private Mock<IPlanningModuleNotificationService> _planningModuleNotificationServiceMock;
        private Mock<ILogService<OperationRequest>> _logServiceMock;
        private Mock<IStaffRepository> _staffRepositoryMock;
        private Mock<IPatientRepository> _patientRepositoryMock;
        private Mock<IOperationRequestRepository> _operationRequestRepositoryMock;
        private Mock<IOperationTypeRepository> _operationTypeRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<OperationRequestService>> _loggerMock;

        private OperationRequestService _operationRequestService;
        private Mock<IOperationRequestCodeGenerator> _operationRequestCodeGenerator;

        [SetUp]
        public void Setup()
        {
            _operationRequestPolicyMock = new Mock<IOperationRequestPolicy>();
            _planningModuleNotificationServiceMock = new Mock<IPlanningModuleNotificationService>();
            _logServiceMock = new Mock<ILogService<OperationRequest>>();
            _staffRepositoryMock = new Mock<IStaffRepository>();
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _operationRequestRepositoryMock = new Mock<IOperationRequestRepository>();
            _operationTypeRepositoryMock = new Mock<IOperationTypeRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<OperationRequestService>>();
            _operationRequestCodeGenerator = new Mock<IOperationRequestCodeGenerator>();

            _operationRequestService = new OperationRequestService(
                _operationRequestPolicyMock.Object,
                _planningModuleNotificationServiceMock.Object,
                _logServiceMock.Object,
                _staffRepositoryMock.Object,
                _patientRepositoryMock.Object,
                _operationRequestRepositoryMock.Object,
                _operationTypeRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                _operationRequestCodeGenerator.Object
            );
        }

        

        [Test]
        public async Task GetOperationRequest_InvalidId_ReturnsError()
        {
            // Arrange
            var operationRequestId = Guid.NewGuid();
            _operationRequestRepositoryMock.Setup(r => r.GetByIdAsync(operationRequestId))
                .ReturnsAsync((OperationRequest)null);

            // Act
            var result = await _operationRequestService.GetOperationRequest(operationRequestId);

            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo($"The operation request with id: {operationRequestId} was not found"));
        }
    }
}
