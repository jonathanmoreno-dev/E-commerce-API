using System.Security.Claims;
using Ecommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Infrastructure.Authentication
{
    public class CurrentUserService : ICurrentUserService
    {
        public Guid UserId { get => Guid.Parse(_httpContextAcessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);}
        public string Name { get => _httpContextAcessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!; }
        public bool IsAdmin { get => _httpContextAcessor.HttpContext!.User.IsInRole("Admin"); }

        private readonly IHttpContextAccessor _httpContextAcessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAcessor = httpContextAccessor;
        }
    }
}
