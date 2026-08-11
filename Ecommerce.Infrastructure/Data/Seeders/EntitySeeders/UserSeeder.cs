using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Domain.ValueObjects;
using Ecommerce.Fakers;

namespace Ecommerce.Infrastructure.Data.Seeders.EntitySeeders
{
    public class UserSeeder
    {
        private readonly AppDbContext _appDbContext;
        private readonly IPasswordHasher _passwordHasher;
        public UserSeeder(AppDbContext appDbContext, IPasswordHasher passwordHasher)
        {
            _appDbContext = appDbContext;
            _passwordHasher = passwordHasher;
        }
        public Task SeedAsync()
        {
            for (int i = 0; i < 100; i++)
            {
                var user = UserFaker.Create();
                var emailPrefix = user.Email.Value.Split('@')[0];
                var passwordHash = _passwordHasher.HashPassword($"{emailPrefix}Password");
                user.ChangePasswordHash(passwordHash);
                user.ChangeAvatarImage(new AvatarImage(UserFaker.GetAvatarImageUrl()));
                _appDbContext.Users.Add(user);
            }
            return Task.CompletedTask;
        }
    }
}
