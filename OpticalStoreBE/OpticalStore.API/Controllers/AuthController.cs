using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpticalStore.API.Requests.Auth;
using OpticalStore.API.Responses.Auth;
using OpticalStore.BLL.DTOs;
using OpticalStore.BLL.Services.Interfaces;

namespace OpticalStore.API.Controllers
{
    [ApiController]
    [Route("auth")]
    [Tags("1. Authentication")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest? request)
        {
            if (request == null)
            {
                return BadRequest("Request body is required.");
            }

            var dto = new RegisterRequestDto
            {
                Dob = request.Dob,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                Password = request.Password,
                Phone = request.Phone
            };

            await _authService.RegisterAsync(dto);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest? request)
        {
            if (request == null)
            {
                return BadRequest("Request body is required.");
            }

            var dto = new LoginRequestDto
            {
                Email = request.Email,
                Password = request.Password
            };

            var result = await _authService.LoginAsync(dto);
            return Ok(ToAuthResponse(result));
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] RefreshTokenRequest? request)
        {
            if (request == null)
            {
                return BadRequest("Request body is required.");
            }

            var result = await _authService.RefreshTokenAsync(request.RefreshToken);
            return Ok(ToAuthResponse(result));
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            await _authService.RevokeRefreshTokenAsync(userId);
            return Ok();
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserResponse?>> Me()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var currentUser = await _authService.GetCurrentUserAsync(userId);
            if (currentUser == null)
            {
                return NotFound();
            }

            return Ok(ToUserResponse(currentUser));
        }

        private string? GetCurrentUserId()
        {
            var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(ClaimTypes.Name)
                ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            return string.IsNullOrWhiteSpace(sub) ? null : sub;
        }

        private static AuthResponse ToAuthResponse(AuthResultDto dto)
        {
            return new AuthResponse
            {
                AccessToken = dto.AccessToken,
                RefreshToken = dto.RefreshToken,
                ExpiresIn = dto.ExpiresIn,
                User = dto.User == null ? null : ToUserResponse(dto.User)
            };
        }

        private static UserResponse ToUserResponse(UserDto dto)
        {
            return new UserResponse
            {
                Id = dto.Id,
                Dob = dto.Dob,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                Phone = dto.Phone,
                ImageUrl = dto.ImageUrl,
                Status = dto.Status
            };
        }

    }
}
