namespace Ecommerce.Domain.Enums
{
    public enum PaymentStatus
    {
        Pending = 1,
        Authorized = 2,
        Completed = 3,
        Failed = 4,
        Canceled = 5,
        Abandoned = 6
    }
}
