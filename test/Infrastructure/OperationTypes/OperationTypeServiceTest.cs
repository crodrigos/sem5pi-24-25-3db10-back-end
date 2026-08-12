using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.OperationTypes.DTO;
using dddnet8.Domain.OperationTypes.Names;
using dddnet8.Domain.RequiredStaffs;
using dddnet8.Domain.RequiredStaffs.DTO;
using dddnet8.Domain.Shared;
using dddnet8.Domain.Specializations;
using dddnet8.Infraestructure.OperationTypes;
using dddnet8.Infraestructure.RequiredStaffs;
using dddnet8.Infraestructure.Shared.Exceptions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace test.Infrastructure.OperationTypes
{
    public class OperationTypeServiceTest
    {
        Mock<IOperationTypeRepository> _operationTypeRepositoryMock;
        Mock<IRequiredStaffRepository> _requiredStaffRepositoryMock;
        Mock<IOperationTypeCodeGenerator> _operationTypeCodeGeneratorMock;
        Mock<IUnitOfWork> _unitOfWorkMock;
        
        IOperationTypeService _operationTypeService;

        List<OperationType> _operationTypes;
        List<RequiredStaff> _requiredStaffs;


        /*[SetUp]
        public void Setup()
        {
            _operationTypeRepositoryMock = new Mock<IOperationTypeRepository>();
            _requiredStaffRepositoryMock = new Mock<IRequiredStaffRepository>();
            _operationTypeCodeGeneratorMock = new Mock<IOperationTypeCodeGenerator>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _operationTypeService = new OperationTypeService(
                _operationTypeRepositoryMock.Object,
                _requiredStaffRepositoryMock.Object,
                _operationTypeCodeGeneratorMock.Object,
                _unitOfWorkMock.Object
            );

            setupData();
        }

        private void setupData()
        {
            // Example Data
            var operationType1 = new OperationType(
                Guid.NewGuid(),
                new Name("Transplante de Pele e Tecidos Moles"),
                Status.Active,
                new EstimatedDuration(new TimeSpan(0, 60, 0),new TimeSpan(0, 60, 0),new TimeSpan(0, 60, 0)),
                OperationTypeCode.Create("OT0001")
            );

            var requiredStaff1_1 = new RequiredStaff(
                Guid.NewGuid(),
                new Specialization("Dermatology", ""),
                new RequiredStaffQuantity(1),
                operationType1
            );

            var requiredStaff1_2 = new RequiredStaff(
                Guid.NewGuid(),
                new Specialization("Cardiology", ""),
            new RequiredStaffQuantity(2),
                operationType1
            );

            _operationTypes = new List<OperationType> { operationType1 };
            _requiredStaffs = new List<RequiredStaff> { requiredStaff1_1, requiredStaff1_2 };

            // Setup Mock
            _operationTypeRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<OperationType> { operationType1 });
            _requiredStaffRepositoryMock
                .Setup(x => x.GetByOperationTypeAsync(operationType1))
                .ReturnsAsync(new List<RequiredStaff> { requiredStaff1_1, requiredStaff1_2 });
        }*/

        // TODO: FIX THIS
        /*[Test]
        public void CheckIfReturnsAllOperationTypesMappedCorrectly()
        {
            List<OperationTypeDTO> expected = new List<OperationTypeDTO>
            {
                new OperationTypeDTO
                {
                    Id = _operationTypes[0].Id.ToString(),
                    Name = _operationTypes[0].Name.Value,
                    Status = (int)_operationTypes[0].Status,
                    EstimatedDuration = (int)
                        _operationTypes[0].EstimatedDuration.CleaningDuration.TotalMinutes,
                    RequiredStaff = new List<RequiredStaffDTO>
                    {
                        new RequiredStaffDTO
                        {
                            Id = _requiredStaffs[0].Id.ToString(),
                            Specialization = (int)_requiredStaffs[0].specialization,
                            SpecializationName = _requiredStaffs[0].specialization.ToString(),
                            Quantity = _requiredStaffs[0].quantity.Value,
                        },
                        new RequiredStaffDTO
                        {
                            Id = _requiredStaffs[1].Id.ToString(),
                            Specialization = (int)_requiredStaffs[1].specialization,
                            SpecializationName = _requiredStaffs[1].specialization.ToString(),
                            Quantity = _requiredStaffs[1].quantity.Value,
                        },
                    },
                },
            };

        var result = _operationTypeService.GetAll().Result;

        // Serialize result and excepted to compare
        Assert.That(
            JsonConvert.SerializeObject(expected) == JsonConvert.SerializeObject(result),
            Is.True
        );
    }*/


        /*public void CheckIfReturnsOperationTypeById()
        {
            var operationType = _operationTypes[0];
            var requiredStaffs = _requiredStaffs;

            _operationTypeRepositoryMock
                .Setup(x => x.GetByIdAsync(operationType.Id))
                .ReturnsAsync(operationType);
            _requiredStaffRepositoryMock
                .Setup(x => x.GetByOperationTypeAsync(operationType))
                .ReturnsAsync(requiredStaffs);

            var expected = new OperationTypeDTO
            {
                Id = operationType.Id.ToString(),
                Name = operationType.Name.Value,
                Status = (int) operationType.Status,
                EstimatedDuration = (int)operationType.EstimatedDuration.CleaningDuration.TotalMinutes,
                RequiredStaff = requiredStaffs
                    .Select(rs => new RequiredStaffDTO
                    {
                        Id = rs.Id.ToString(),
                        Specialization = (int)rs.specialization,
                        SpecializationName = rs.specialization.ToString(),
                        Quantity = rs.quantity.Value,
                    })
                    .ToList(),
            };

        var result = _operationTypeService.GetById(operationType.Id.ToString()).Result;

        Assert.That(
        JsonConvert.SerializeObject(expected) == JsonConvert.SerializeObject(result),
        Is.True
        );
    }*/
        
        // FIX IT
/*
        [Test]
        public void CheckIfReturnsOperationTypesByStatus()
        {
            var operationType = _operationTypes[0];
            var requiredStaffs = _requiredStaffs;

            _operationTypeRepositoryMock
                .Setup(x => x.GetByStatusAsync(Status.Active))
                .ReturnsAsync(new List<OperationType> { operationType });
            _requiredStaffRepositoryMock
                .Setup(x => x.GetByOperationTypeAsync(operationType))
                .ReturnsAsync(requiredStaffs);

            var expected = new List<OperationTypeDTO>
            {
                new OperationTypeDTO
                {
                    Id = operationType.Id.ToString(),
                    Name = operationType.Name.Value,
                    Status = (int)operationType.Status,
                    EstimatedDuration = (int)operationType.EstimatedDuration.CleaningDuration.TotalMinutes,
                    RequiredStaff = requiredStaffs
                        .Select(rs => new RequiredStaffDTO
                        {
                            Id = rs.Id.ToString(),
                            Specialization = (int)rs.specialization,
                            SpecializationName = rs.specialization.ToString(),
                            Quantity = rs.quantity.Value,
                        })
                        .ToList(),
                },
            };

            var result = _operationTypeService.GetByStatus((int)Status.Active).Result;

            Assert.That(
                JsonConvert.SerializeObject(expected) == JsonConvert.SerializeObject(result),
                Is.True
            );
        }*/

/*
[Test]
public void CheckIfAddOperationType()
{
    var operationTypeDTO = new OperationTypeDTO
    {
        Id = Guid.NewGuid().ToString(),
        Name = "New Operation",
        Status = (int) Status.Active,
        EstimatedDuration = 90,
        RequiredStaff = new List<RequiredStaffDTO>
        {
            new RequiredStaffDTO
            {
                Id = Guid.NewGuid().ToString(),
                Specialization = (int)Specialization.Dermatology,
                SpecializationName = Specialization.Dermatology.ToString(),
                Quantity = 1,
            },
        },
    };

    var operationType = new OperationType(
        Guid.Parse(operationTypeDTO.Id),
        new Name(operationTypeDTO.Name),
        Status.Active,
        new EstimatedDuration(new TimeSpan(0, operationTypeDTO.EstimatedDuration, 0))
    );

    var requiredStaff = new RequiredStaff(
        Guid.Parse(operationTypeDTO.RequiredStaff[0].Id),
        Specialization.Dermatology,
        new RequiredStaffQuantity(operationTypeDTO.RequiredStaff[0].Quantity),
        operationType
    );

    _operationTypeRepositoryMock
        .Setup(x => x.AddAsync(It.IsAny<OperationType>()))
        .ReturnsAsync(operationType);
    _requiredStaffRepositoryMock
        .Setup(x => x.AddAsync(It.IsAny<RequiredStaff>()))
        .ReturnsAsync(requiredStaff);

    var result = _operationTypeService.Add(operationTypeDTO).Result;

    Assert.That(result.Name == operationTypeDTO.Name);
    Assert.That(result.EstimatedDuration == operationTypeDTO.EstimatedDuration);
    Assert.That(result.RequiredStaff.Count == operationTypeDTO.RequiredStaff.Count);
}
*/

/*[Test]
public void CheckIfThrownMultipleArgumentException()
{
    var addDto = new OperationTypeAddDTO
    {
        Name = "",
        Status = 4,
        EstimatedDuration = 0,
        RequiredStaff = new List<RequiredStaffAddDTO>
        {
            new RequiredStaffAddDTO()
            {
                Quantity = 1,
                Specialization = 2
            }
        },
    };

    Assert.That(() => _operationTypeService.Add(addDto), Throws.InstanceOf<MultipleArgumentException>());

}*/
    }
}