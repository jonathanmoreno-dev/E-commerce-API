using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        public Guid UserId { get; }
        public string Name { get; }
        public UserRole Role { get; }
    }
}
