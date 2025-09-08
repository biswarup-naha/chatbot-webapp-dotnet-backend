using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using platychat_dotnet.DTOs;
using platychat_dotnet.Models;
using platychat_dotnet.Services;
using platychat_dotnet.Utils;

namespace platychat_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IUserService service) : ControllerBase
    {
        private readonly IUserService _service = service;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var exists = await _service.GetUserByEmail(registerDto.Email);
            if (exists != null)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "user already exists",
                });
            }

            var newUser = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                Password = registerDto.Password
            };

            await _service.CreateUser(newUser);
            var token=Jwt.GenerateToken(new UserDto
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                Chats = newUser.Chats,
            });

            return Ok(new ApiResponse<UserDto>
            {
                Success = true,
                Message = "user registered",
                Result = new UserDto
                {
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Email = newUser.LastName,
                    Chats = newUser.Chats,
                },
                Token=token
            });
        }


        #region Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var exists = await _service.GetUserByEmail(loginDto.Email);
            if (exists != null && exists.Password == loginDto.Password)
            {
                var loggedInUser = new UserDto
                {
                    FirstName = exists.FirstName,
                    LastName = exists.LastName,
                    Email = exists.Email,
                    Chats = exists.Chats
                };
                return Ok(new ApiResponse<UserDto>
                {
                    Success = true,
                    Message = "user logged in",
                    Result = loggedInUser
                });
            }
            else
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "failed to log in",
                });
            }
        }
    }
}
#endregion
