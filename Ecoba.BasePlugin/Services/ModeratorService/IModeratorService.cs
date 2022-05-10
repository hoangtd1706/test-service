using Ecoba.BasePlugin.Services.ModeratorService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecoba.BasePlugin.Services.ModeratorService
{
    public interface IModeratorService<TContext>
    {
        Task<IEnumerable<ModeratorModel>> GetModerators();
        Task<bool> Check(string userNumber, string role);
        Task<bool> Create(string userNumber, string role);
        Task<bool> Remove(string userNumber, string role);
    }
}
