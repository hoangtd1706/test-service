using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ecoba.IdentityService.Services.UserService;
using Ecoba.BasePlugin.Data;
using Ecoba.BasePlugin.Services.ModeratorService;
using Ecoba.BasePlugin.Services.PluginConfigService;
using Ecoba.BasePlugin.Services.PluginConfigService.Models;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;

namespace Ecoba.BasePlugin.Controllers
{
    public class BasePluginConfigsController<TContext> : ControllerBase where TContext : BaseDbContext
    {
        protected readonly IUserService _userSer;
        protected readonly IModeratorService<TContext> _moderatorSer;
        protected readonly string MOD_ROLE = "MOD_ROLE";
        protected readonly IPluginConfigService<PluginMode, TContext> _pluginModeConfigSer;
        protected readonly IPluginConfigService<BlockUser, TContext> _pluginBlockUserConfigSer;

        public BasePluginConfigsController(
            IModeratorService<TContext> moderatorSer,
            IUserService userSer,
            IPluginConfigService<PluginMode, TContext> pluginModeConfigSer,
            IPluginConfigService<BlockUser, TContext> pluginBlockUserConfigSer
            )
        {
            _moderatorSer = moderatorSer;
            _userSer = userSer;
            _pluginModeConfigSer = pluginModeConfigSer;
            _pluginBlockUserConfigSer = pluginBlockUserConfigSer;
        }

        // GET: api/Configs
        [HttpGet("[action]")]
        public async Task<ActionResult<bool>> GetPluginMode()
        {
            var userNumber = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            if (!await _moderatorSer.Check(userNumber, MOD_ROLE))
                return Forbid();

            var pluginMode = await _pluginModeConfigSer.GetConfig();
            if (pluginMode == null)
                return false;

            return pluginMode.AllowAll;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SetPluginMode(PluginMode config)
        {
            var userNumber = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            if (!await _moderatorSer.Check(userNumber, MOD_ROLE))
                return Forbid();

            await _pluginModeConfigSer.SetConfig(config);

            return NoContent();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<BlockUserModel>>> GetBlockUsers()
        {
            var userNumber = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            if (!await _moderatorSer.Check(userNumber, MOD_ROLE))
                return Forbid();

            var blockUser = await _pluginBlockUserConfigSer.GetConfig();
            if (blockUser == null)
                return new List<BlockUserModel>();

            var result = new List<BlockUserModel>();
            var users = await _userSer.GetAll();
            foreach (var item in blockUser.UserNumbers)
            {
                var user = users.FirstOrDefault(x => x.EmployeeId == item);
                result.Add(new BlockUserModel()
                {
                    UserNumber = item,
                    FullName = user == null ? "" : user.DisplayName
                });
            }
            return result.ToList();
        }

        [HttpPost("[action]/{userNumber}")]
        public async Task<IActionResult> AddBlockUser([FromRoute] string userNumber)
        {
            var modUserNumber = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            if (!await _moderatorSer.Check(modUserNumber, MOD_ROLE))
                return Forbid();

            var blockUser = await _pluginBlockUserConfigSer.GetConfig();
            if (blockUser == null)
            {
                await _pluginBlockUserConfigSer.SetConfig(
                    new BlockUser { UserNumbers = new List<string> { userNumber } });
            }
            else
            {
                if (!blockUser.UserNumbers.Contains(userNumber))
                {
                    var list = blockUser.UserNumbers.ToList();
                    list.Add(userNumber);
                    await _pluginBlockUserConfigSer.SetConfig(
                    new BlockUser { UserNumbers = list });
                }
            }

            return NoContent();
        }
    }

    public class BlockUserModel
    {
        public string UserNumber { get; set; }
        public string FullName { get; set; }
    }
}
