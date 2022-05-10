using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ecoba.IdentityService.Services.UserService;
using Ecoba.TestService.Data;
using Ecoba.BasePlugin.Controllers;
using Ecoba.BasePlugin.Services.ModeratorService;

namespace Ecoba.TestService.Controller.Plugin
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ModeratorsController : BaseModeratorsController<TestDbContext>
    {
        public ModeratorsController(IUserService userSer, IModeratorService<TestDbContext> moderatorSer) : base(userSer, moderatorSer)
        {
        }
    }
}
