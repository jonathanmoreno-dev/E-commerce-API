using E_commerce_API.src.Domain.Enums;
using E_commerce_API.src.Domain.ValueObjects;

namespace E_commerce_API.src.Domain.Entities
{
    public class Payment
    {
        public int PaymentId { get; private set; }
        public int OrderId { get; private set; }
        public Order Order { get; private set; } = null!;
        public Money Amount { get; private set; } = null!;
        public DateTime PaymentDate { get; private set; }
        public PaymentMethod Method { get; private set; }
        public PaymentStatus Status { get; private set; }

        private Payment() { }
        public Payment(decimal amount, PaymentMethod method)
        {
            Amount = new Money(amount);
            Status = PaymentStatus.Pending;
            Method = method;
            PaymentDate = DateTime.UtcNow;
        }
        public void MarkAsAuthorized()
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Only pending payments can be authorized");

            Status = PaymentStatus.Authorized;
        }
        public void MarkAsCompleted()
        {
            if (Status != PaymentStatus.Authorized)
                throw new InvalidOperationException("Only authorized payments can be completed");

            Status = PaymentStatus.Completed;
        }
        public void MarkAsFailed()
        {
            if (Status != PaymentStatus.Pending && Status != PaymentStatus.Authorized)
                throw new InvalidOperationException("Only pending or authorized payments can be failed");

            Status = PaymentStatus.Failed;
        }
        public void MarkAsCanceled()
        {
            if (Status != PaymentStatus.Pending && Status != PaymentStatus.Authorized)
                throw new InvalidOperationException("Only pending or authorized payments can be canceled");

            Status = PaymentStatus.Canceled;
        }
        public void MarkAsAbandoned()
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Only pending payments can be abandoned");

            Status = PaymentStatus.Abandoned;
        }
    }
}
