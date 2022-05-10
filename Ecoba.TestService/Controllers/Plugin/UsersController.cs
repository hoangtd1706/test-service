using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ecoba.BasePlugin.Services.ModeratorService;
using Ecoba.BasePlugin.Controllers;
using Ecoba.BasePlugin.Services.PluginConfigService;
using Ecoba.IdentityService.Services.UserService;
using Ecoba.BasePlugin.Services.PluginConfigService.Models;
using Ecoba.TestService.Data;

namespace Ecoba.TestService.Controller.Plugin
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : BaseUsersController<TestDbContext>
    {
        public UsersController(
            IModeratorService<TestDbContext> moderatorSer,
            IUserService userSer,
            IPluginConfigService<PluginMode, TestDbContext> pluginModeConfigSer,
            IPluginConfigService<BlockUser, TestDbContext> pluginBlockUserConfigSer
            )
            : base(moderatorSer, userSer, pluginModeConfigSer, pluginBlockUserConfigSer)
        {
        }
        protected override string[] GetRoles()
        {
            return new string[]
            {
                "EDIT_ROLE",
                "APPROVE_ROLE",
            };
        }
    }
}
