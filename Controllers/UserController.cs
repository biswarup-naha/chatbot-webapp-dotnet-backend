using Microsoft.AspNetCore.Authorization;
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
    public class UserController(IUserService service) : ControllerBase
    {
        private readonly IUserService _service = service;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = await _service.GetAllUsers();
                return Ok(new ApiResponse<List<User>>
                {
                    Success = true,
                    Message = "users fetched",
                    Result = users
                });
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Success = false,
                });
            }
        }
        
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var user = await _service.GetUserById(id);
                return Ok(new ApiResponse<User>
                {
                    Success = true,
                    Message = "user fetched",
                    Result = user
                });
            }
            catch (System.Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Success = false,
                    Message=e.Message
                });
            }
        }

        
    }
}
