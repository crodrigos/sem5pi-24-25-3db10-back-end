using NUnit.Framework;

using dddnet8.Domain.OperationTypes;

namespace test.Domain.OperationTypes.ValueObjects
{
    public class EstimatedDurationTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void InstanciatesCorrectly()
        {
            EstimatedDuration estimatedDuration = new EstimatedDuration(new System.TimeSpan(1, 30, 0),new System.TimeSpan(1, 30, 0),new System.TimeSpan(1, 30, 0));
        }
        
        [Test]
        public void ThrowsExceptionWhenDurationIsZero()
        {
            Assert.Throws<System.ArgumentException>(() => {
                new EstimatedDuration(new System.TimeSpan(0, -1, 0),new System.TimeSpan(1, 30, 0),new System.TimeSpan(1, 30, 0));
            });
        }
    }
}