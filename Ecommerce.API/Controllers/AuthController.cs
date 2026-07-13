using Ecommerce.Application.DTOs.Authentication;
using Ecommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register(RegisterRequestDTO requestDTO)
        {
            var authResponseDTO = await _authService.RegisterAsync(requestDTO);
            return Ok(authResponseDTO);
        }
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginRequestDTO requestDTO)
        {
            var authResponseDTO = await _authService.LoginAsync(requestDTO);
            return Ok(authResponseDTO);
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDTO>> RefreshToken(RefreshTokenRequestDTO requestDTO)
        {
            var authResponseDTO = await _authService.RefreshTokenAsync(requestDTO.RefreshToken);
            return Ok(authResponseDTO);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshTokenRequestDTO requestDTO)
        {
            await _authService.LogoutAsync(requestDTO.RefreshToken);
            return NoContent();
        }
    }
}
