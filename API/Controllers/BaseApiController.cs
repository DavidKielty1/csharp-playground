using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API;

[ServiceFilter(typeof(LogUserActivity))]
[ApiController]
[Route("api/[controller]")] // /api/users
public class BaseApiController : ControllerBase
{
    
}
