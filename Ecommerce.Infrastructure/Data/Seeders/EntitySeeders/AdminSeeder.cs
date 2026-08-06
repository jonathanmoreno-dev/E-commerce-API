using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data.Seeders.EntitySeeders
{
    public class AdminSeeder
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        public AdminSeeder(
            AppDbContext context,
            IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task SeedAsync()
        {
            if (await _context.Users.AnyAsync(x => x.Role == UserRole.Admin))
                return;

            var passwordHash = _passwordHasher.HashPassword("Admin@123");

            var admin = new User(
                new PersonName("Administrador"),
                new Email("admin@gmail.com"),
                new PhoneNumber("54999999999"),
                passwordHash);

            admin.ChangeRole(UserRole.Admin);

            _context.Users.Add(admin);
            _context.Carts.Add(new Cart(admin.Id));

            await _context.SaveChangesAsync();
        }
    }
}
