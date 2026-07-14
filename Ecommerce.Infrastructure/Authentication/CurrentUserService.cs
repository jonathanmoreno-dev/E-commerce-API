using System.Security.Claims;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Infrastructure.Authentication
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid UserId { get => Guid.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);}
        public string Name { get => _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!; }
        public UserRole Role { get => Enum.Parse<UserRole>(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Role)!); }
    }
}
