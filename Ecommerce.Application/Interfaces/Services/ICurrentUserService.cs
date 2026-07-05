namespace Ecommerce.Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        public Guid UserId { get; }
        public string Name { get; }
        public bool IsAdmin { get; }
    }
}
