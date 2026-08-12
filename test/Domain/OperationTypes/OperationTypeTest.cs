using NUnit.Framework;

using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.OperationTypes.Names;

namespace test.Domain.OperationTypes
{
    public class OperationTypeTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Description("OperationType instanciates correctly")]
        public void InstanciatesCorrectly()
        {
            Guid id = Guid.NewGuid();
            Name name = new Name("Transplante de Coração");
            EstimatedDuration estimatedDuration = new EstimatedDuration(new TimeSpan(1, 30, 0),new System.TimeSpan(1, 30, 0),new System.TimeSpan(1, 30, 0));

            OperationType operationType = new OperationType(id, name, Status.Active, estimatedDuration, OperationTypeCode.Create("OT0001"));
        }
    }
}