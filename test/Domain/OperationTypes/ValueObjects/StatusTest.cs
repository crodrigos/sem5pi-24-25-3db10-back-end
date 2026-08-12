using dddnet8.Domain.OperationTypes;
using NUnit.Framework;

namespace test.Domain.OperationTypes.ValueObjects
{
    public class StatusTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void StatusActiveMapCorrectlyToItsIntegerValue_Active()
        {
            int expected = 1;
            var result = (int) Status.Active;

            Assert.That(result, Is.EqualTo(expected));
        }

        public void StatusActiveMapCorrectlyToItsIntegerValue_Inactive()
        {
            int expected = 0;
            var result = (int) Status.Inactive;
            
            Assert.That(result, Is.EqualTo(expected));
        }
        
    }
}