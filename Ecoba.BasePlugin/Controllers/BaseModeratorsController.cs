using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ecoba.IdentityService.Services.UserService;
using Ecoba.BasePlugin.Services.ModeratorService;
using Ecoba.BasePlugin.Services.ModeratorService.Models;
using System.Security.Claims;
using Ecoba.BasePlugin.Data;

namespace Ecoba.BasePlugin.Controllers
{
    public class BaseModeratorsController<TContext> : ControllerBase where TContext : BaseDbContext
    {
        protected readonly IUserService _userSer;
        protected readonly IModeratorService<TContext> _moderatorSer;
        protected readonly string MOD_ROLE = "MOD_ROLE";

        public BaseModeratorsController(IUserService userSer, IModeratorService<TContext> moderatorSer)
        {
            _userSer = userSer;
            _moderatorSer = moderatorSer;
        }

        // GET: api/Moderators
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModeratorModel>>> GetModerators()
        {
            if (!_userSer.CheckAdmin())
                return Forbid();

            var result = await _moderatorSer.GetModerators();

            return result.ToList();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<bool>> CheckPermission()
        {
            var userNumber = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            return await _moderatorSer.Check(userNumber, MOD_ROLE);
        }

        // POST: api/Moderators
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("[action]/{userNumber}")]
        public async Task<ActionResult> CreateMod([FromRoute] string userNumber)
        {
            if (!_userSer.CheckAdmin())
                return Forbid();

            var result = await _moderatorSer.Create(userNumber, MOD_ROLE);
            if (result)
                return Ok();

            return BadRequest();
        }

        // DELETE: api/Moderators/5
        [HttpDelete("[action]/{userNumber}")]
        public async Task<ActionResult> RemoveMod([FromRoute] string userNumber)
        {
            if (!_userSer.CheckAdmin())
                return Forbid();

            var result = await _moderatorSer.Remove(userNumber, MOD_ROLE);
            if (result)
                return Ok();

            return BadRequest();
        }
    }
}