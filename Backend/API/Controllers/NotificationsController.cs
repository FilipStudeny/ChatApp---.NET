using API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Models;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationsController : ControllerBase
{
    public NotificationsController()
    {
        
    }
    
    [HttpGet]
    [Route("/notifications/user/:id")]
    public async Task<ActionResult<ServiceResponse<List<User>>>> GetUsers()
    {
        throw new NotImplementedException();
    }
}