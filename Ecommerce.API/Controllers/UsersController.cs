using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.DTOs.ShippingDTOs;
using Ecommerce.Application.DTOs.UserDTOs;
using Ecommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("me")]
        public async Task<ActionResult<UserDetailsDTO>> GetCurrent(CancellationToken cancellationToken)
        {
            var user = await _userService.GetCurrentAsync(cancellationToken);
            return Ok(user);
        }
        [HttpPatch("me")]
        public async Task<ActionResult<UserDetailsDTO>> Update(UserUpdateDTO requestDTO, CancellationToken cancellationToken)
        {
            var user = await _userService.UpdateAsync(requestDTO, cancellationToken);
            return Ok(user);
        }
        [HttpPatch("me/password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO requestDTO, CancellationToken cancellationToken)
        {
            await _userService.ChangePasswordAsync(requestDTO, cancellationToken);
            return NoContent();
        }
        [HttpPost("me/shipping-addresses")]
        public async Task<ActionResult<UserDetailsDTO>> AddShippingAddress(ShippingAddressDTO requestDTO, CancellationToken cancellationToken)
        {
            var user = await _userService.AddShippingAddressAsync(requestDTO, cancellationToken);
            return Ok(user);
        }
        [HttpDelete("me/shipping-addresses")]
        public async Task<ActionResult<UserDetailsDTO>> RemoveShippingAddress(ShippingAddressDTO requestDTO, CancellationToken cancellationToken)
        {
            var user = await _userService.RemoveShippingAddressAsync(requestDTO, cancellationToken);
            return Ok(user);
        }
    }
}
