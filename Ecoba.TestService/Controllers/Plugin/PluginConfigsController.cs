using Microsoft.AspNetCore.Mvc;
using Ecoba.IdentityService.Services.UserService;
using Ecoba.BasePlugin.Services.ModeratorService;
using Ecoba.BasePlugin.Services.PluginConfigService;
using Ecoba.BasePlugin.Services.PluginConfigService.Models;
using Microsoft.AspNetCore.Authorization;
using Ecoba.BasePlugin.Controllers;
using Ecoba.TestService.Data;

namespace Ecoba.TestService.Controller.Plugin
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class PluginConfigsController : BasePluginConfigsController<TestDbContext>
    {
        public PluginConfigsController(
            IModeratorService<TestDbContext> moderatorSer,
            IUserService userSer,
            IPluginConfigService<PluginMode, TestDbContext> pluginModeConfigSer,
            IPluginConfigService<BlockUser, TestDbContext> pluginBlockUserConfigSer
            )
            : base(moderatorSer, userSer, pluginModeConfigSer, pluginBlockUserConfigSer)
        {
        }
    }
}
