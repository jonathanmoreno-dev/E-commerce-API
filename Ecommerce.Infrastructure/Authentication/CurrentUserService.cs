using System.Security.Claims;
using Ecommerce.Application.Interfaces.Services;
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
        public bool IsAdmin { get => _httpContextAccessor.HttpContext!.User.IsInRole("Admin"); }
    }
}
