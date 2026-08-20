using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.ValueObjects
{
    public class PersonNameTests
    {
        [Fact]
        public void ShouldBeEqualWhenNamesAreTheSame()
        {
            var name = new string('a', 10) + new string('b', 14) + new string('c', 7);

            var personName1 = new PersonName(name);
            var personName2 = new PersonName(name);

            Assert.Equal(personName1, personName2);
        }
        [Fact]
        public void ShouldNotBeEqualWhenNamesAreDifferent()
        {
            var name1 = new string('a', 10) + new string('b', 14) + new string('c', 7);
            var name2 = new string('d', 10) + new string('e', 14) + new string('f', 7);

            var personName1 = new PersonName(name1);
            var personName2 = new PersonName(name2);

            Assert.NotEqual(personName1, personName2);
        }
        [Fact]
        public void ShouldCreateValidPersonName()
        {
            var name = new string('b', 16);

            var personName = new PersonName(name);

            Assert.Equal(name, personName.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldThrowDomainValidationExceptionWhenNameIsNullOrWhiteSpace(string name)
        {
            Assert.Throws<DomainValidationException>(() => new PersonName(name));
        }

        [Fact]
        public void ShouldTrimNameBeforeCreatingPersonName()
        {
            var name = "Exemplo de Nome";
            var personName = new PersonName($"    {name}    ");

            Assert.Equal(name, personName.Value);
        }
        [Fact]
        public void ShouldThrowDomainValidationExceptionWhenNameIsMoreThan150Characters()
        {
            var name = new string('a', 151);

            Assert.Throws<DomainValidationException>(() => new PersonName(name));
        }
        [Fact]
        public void ShouldCreatePersonNameWithExactly150Characters()
        {
            var name = new string('a', 150);

            var personName = new PersonName(name);

            Assert.Equal(name, personName.Value);
        }

    }
}