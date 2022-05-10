using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ecoba.BasePlugin.Services.ModeratorService;
using Ecoba.BasePlugin.Services.ModeratorService.Models;
using System.Security.Claims;
using Ecoba.BasePlugin.Data;
using Ecoba.IdentityService.Services.UserService;
using Ecoba.BasePlugin.Services.PluginConfigService;
using Ecoba.BasePlugin.Services.PluginConfigService.Models;

namespace Ecoba.BasePlugin.Controllers
{
    public abstract class BaseUsersController<TContext> : ControllerBase where TContext : BaseDbContext
    {
        protected readonly IUserService _userSer;
        protected readonly IModeratorService<TContext> _moderatorSer;
        protected readonly string MOD_ROLE = "MOD_ROLE";
        protected readonly IPluginConfigService<PluginMode, TContext> _pluginModeConfigSer;
        protected readonly IPluginConfigService<BlockUser, TContext> _pluginBlockUserConfigSer;

        public BaseUsersController(
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

        // GET: api/Moderators
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModeratorModel>>> GetUsers()
        {
            var userNumber = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            if (!await _moderatorSer.Check(userNumber, MOD_ROLE))
                return Forbid();

            var result = await _moderatorSer.GetModerators();

            return result.Where(x => x.Role != MOD_ROLE).ToList();
        }

        [HttpGet("[action]")]
        public virtual async Task<ActionResult<bool>> CheckPermission()
        {
            var userNumber = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var pluginMode = await _pluginModeConfigSer.GetConfig();
            if (pluginMode != null && pluginMode.AllowAll)
            {
                var blockUser = await _pluginBlockUserConfigSer.GetConfig();
                if (blockUser == null)
                {
                    return true;
                }
                else
                {
                    if (blockUser.UserNumbers.Contains(userNumber)) { return false; }
                    else { return true; }
                }
            }
            else
            {
                var result = await _moderatorSer.GetModerators();
                return result.Any(x => x.UserNumber == userNumber);
            }
        }


        [HttpGet("[action]")]
        public async Task<ActionResult<bool>> CheckRolePermission([FromQuery] string role)
        {
            var _roles = GetRoles();
            if (!_roles.Contains(role))
                return false;

            var userNumber = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            return await _moderatorSer.Check(userNumber, role);
        }

        [HttpPost("[action]/{userNumber}")]
        public async Task<ActionResult> Create([FromRoute] string userNumber, [FromQuery] string role)
        {
            var _roles = GetRoles();
            if (!_roles.Contains(role))
                return BadRequest();

            var _userNumber = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            if (!await _moderatorSer.Check(_userNumber, MOD_ROLE))
                return Forbid();

            var result = await _moderatorSer.Create(userNumber, role);
            if (result)
                return Ok();

            return BadRequest();
        }

        // DELETE: api/Moderators/5
        [HttpDelete("[action]/{userNumber}")]
        public async Task<ActionResult> Remove([FromRoute] string userNumber, [FromQuery] string role)
        {
            var _roles = GetRoles();
            if (!_roles.Contains(role))
                return BadRequest();

            var _userNumber = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            if (!await _moderatorSer.Check(_userNumber, MOD_ROLE))
                return Forbid();

            var result = await _moderatorSer.Remove(userNumber, role);
            if (result)
                return Ok();

            return BadRequest();
        }

        protected virtual string[] GetRoles()
        {
            return new string[0];
        }
    }
}
