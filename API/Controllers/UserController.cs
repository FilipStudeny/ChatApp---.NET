using API.Models;
using API.Models.DTOs;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService userService = userService;

        [HttpGet]
        [Route("/")]
        public async Task<ActionResult<ServiceResponse<List<User>>>> GetUsers()
        {
            return await userService.GetUsers();
        }

        [HttpPost]
        [Route("/login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(Login_DTO login_DTO)
        {
            ServiceResponse<string> response = await userService.Login(login_DTO);
            if (!response.Success)
            {
                return StatusCode(401, response);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("/register")]
        public async Task<ActionResult<ServiceResponse<bool>>> Register(Register_DTO register_DTO)
        {
            ServiceResponse<bool> response = await userService.Register(register_DTO);
            if (!response.Success)
            {
                return StatusCode(401, response);
            }

            return Ok(response);
        }
    }
}
