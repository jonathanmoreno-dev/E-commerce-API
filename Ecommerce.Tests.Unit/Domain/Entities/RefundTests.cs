using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Tests.Unit.Domain.Entities
{
    public class RefundTests
    {
        [Fact]
        public void ShouldCreateRefundWithValidValues()
        {
            var quantity = new Quantity(2);

            var refund = new Refund(quantity);

            Assert.NotEqual(Guid.Empty, refund.Id);
            Assert.Equal(quantity, refund.Quantity);
            Assert.NotEqual(DateTime.MinValue, refund.RefundDate);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenQuantityIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Refund(null!));
        }
    }
}
