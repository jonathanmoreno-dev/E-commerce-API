using Ecommerce.Application.DTOs.ProductDTOs;
using Ecommerce.Application.DTOs.UserDTOs;
using Ecommerce.Application.Interfaces.Services;
using Ecommerce.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers.Admins
{
    [ApiController]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [Route("api/admin/users")]
    public class AdminUsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public AdminUsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<ActionResult<UserListDTO>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }
        [HttpGet("role")]
        public async Task<ActionResult<UserListDTO>> GetAllByRole(UserRole role)
        {
            var users = await _userService.GetAllByRoleAsync(role);
            return Ok(users);
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductDetailsDTO>> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }
        [HttpPatch("{id}/role")]
        public async Task<IActionResult> ChangeRole(Guid id, UserRole request)
        {
            await _userService.ChangeRoleAsync(id, request);
            return NoContent();
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
