using Ecoba.BasePlugin.Data;
using Ecoba.BasePlugin.Data.Models;
using Ecoba.BasePlugin.Services.ModeratorService.Models;
using Ecoba.IdentityService.Services.UserService;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ecoba.BasePlugin.Services.ModeratorService
{
    public class ModeratorService<TContext> : IModeratorService<TContext> where TContext : BaseDbContext
    {
        private readonly TContext _context;
        private readonly IUserService _userSer;
        public ModeratorService(TContext context, IUserService userSer)
        {
            _context = context;
            _userSer = userSer;
        }
        public async Task<IEnumerable<ModeratorModel>> GetModerators()
        {
            var result = new List<ModeratorModel>();
            var moderators = await _context.Moderators.ToListAsync();
            var users = await _userSer.GetAll();
            foreach (var moderator in moderators)
            {
                var user = users.FirstOrDefault(x => x.EmployeeId == moderator.UserNumber);
                result.Add(new ModeratorModel()
                {
                    UserNumber = moderator.UserNumber,
                    FullName = user == null ? "" : user.DisplayName,
                    Role = moderator.Role
                });
            }
            return result.ToList();
        }
        public async Task<bool> Check(string userNumber, string role)
        {
            return await _context.Moderators.AnyAsync(x => x.UserNumber == userNumber && x.Role == role);
        }

        public async Task<bool> Create(string userNumber, string role)
        {
            var users = await _userSer.GetAll();
            var user = users.FirstOrDefault(x => x.EmployeeId == userNumber);
            if (user == null)
                return false;

            var exist = await _context.Moderators.FirstOrDefaultAsync(x => x.UserNumber == userNumber && x.Role == role);
            if (exist != null)
                return false;

            _context.Moderators.Add(new Moderator { UserNumber = userNumber, Role = role });
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Remove(string userNumber, string role)
        {
            var users = await _userSer.GetAll();
            var user = users.FirstOrDefault(x => x.EmployeeId == userNumber);
            if (user == null)
                return false;

            var exist = await _context.Moderators.FirstOrDefaultAsync(x => x.UserNumber == userNumber && x.Role == role);
            if (exist == null)
                return false;

            _context.Moderators.Remove(exist);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
