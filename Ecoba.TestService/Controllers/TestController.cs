using System.Security.Claims;
using System.Collections;
namespace Ecoba.TestService.Controller;

using Microsoft.AspNetCore.Mvc;
using Ecoba.IdentityService.Services.UserService;
using Ecoba.IdentityService.Data.Model;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

[Route("api/v1/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IUserService _userSv;

    public TestController(IUserService userSv)
    {
        _userSv = userSv;
    }

    [HttpGet]
    public string Index()
    {
        return "test";
    }

    [HttpGet("users")]
    [Authorize]
    public async Task<IActionResult> getUser()
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        var users = await _userSv.GetAll();
        if (users == null) return BadRequest();
        Console.WriteLine($"UserId: {userId}");
        return Ok(users);
    }

}