using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data
{
    public class AdminSeeder
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;

        public AdminSeeder(IUserRepository userRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }
        public async Task SeedAsync()
        {
            if (await _userRepository.ExistsAdminAsync())
                return;

            var passwordHash = _passwordHasher.HashPassword("Admin@123");
            var user = new User(new PersonName("Administrador"), new Email("admin@gmail.com"), new PhoneNumber("(54) 43 93245-8321"), passwordHash);
            user.ChangeRole(UserRole.Admin);
            _userRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
