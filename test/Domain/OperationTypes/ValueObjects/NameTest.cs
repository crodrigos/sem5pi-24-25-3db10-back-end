using NUnit.Framework;

using dddnet8.Domain.OperationTypes;
using dddnet8.Domain.OperationTypes.Names;

namespace test.Domain.OperationTypes.ValueObjects
{
    public class NameTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void InstanciatesCorrectly()
        {
            Name name = new Name("Transplante de Coração");
        }

        [Test]
        public void ThrowsExceptionWhenNameIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Name(null));
        }

        [Test]
        public void ThrowsExceptionWhenNameIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => new Name(string.Empty));
        }

        [Test]
        public void ThrowsExceptionWhenNameIsWhitespace()
        {
            Assert.Throws<ArgumentException>(() => new Name(" "));
        }

        [Test]
        public void ThrowsExceptionWhenNameIsLongerThanMaxLength()
        {
            Assert.Throws<ArgumentException>(() => new Name("a".PadRight(Name.MaxLength + 1, 'a')));
        }

        [Test]
        public void ReturnsCorrectValue()
        {
            string name_prim = "Test";
            Name name = new Name(name_prim);
            Assert.That(name.Value, Is.EqualTo(name_prim));
        }

        [Test]
        public void EqualsWorksCorrectlyWithNameParameter()
        {
            string name_prim = "Test";  
            Name name1 = new Name(name_prim);
            Name name2 = new Name(name_prim);
            
            Assert.That(name1.Equals(name2), Is.EqualTo(true));
        }

        [Test]
        public void EqualsWorksCorrectlyWithObjectParameter()
        {
            string name_prim = "Test";
            Name name1 = new Name(name_prim);
            object name2 = new Name(name_prim);
            
            Assert.That(name1.Equals(name2), Is.EqualTo(true));
        }

        [Test]
        public void EqualsThrowsExceptionWhenNameIsNull()
        {
            string name_prim = "Test";
            Name name1 = new Name(name_prim);
            
            Assert.That(name1.Equals(null), Is.False);
        }
    }
}