using Ecoba.IdentityService.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecoba.TestService.Controller.Plugin
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userSer;
        public AdminController(IUserService userSer)
        {
            _userSer = userSer;
        }

        [HttpGet]
        public ActionResult<bool> Check()
        {
            return _userSer.CheckAdmin();
        }
    }
}
