using API.Models;
using API.Models.DTOs;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Shared;
using Shared.DTOs;
using Shared.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet]
        [Route("/")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<List<User>>>> GetUsers()
        {
            return await _userService.GetUsers();
        }

        [HttpPost]
        [Route("/login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(LoginDto loginDto)
        {
            var response = await _userService.Login(loginDto);
            return !response.Success ? StatusCode(401, response) : Ok(response);
        }

        [HttpPost]
        [Route("/register")]
        public async Task<ActionResult<ServiceResponse<bool>>> Register(RegisterDto registerDto)
        {
            var response = await _userService.Register(registerDto);
            return !response.Success ? StatusCode(401, response) : Ok(response);
        }
    }
}
