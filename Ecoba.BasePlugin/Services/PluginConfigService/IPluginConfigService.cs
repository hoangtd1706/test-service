using System.Threading.Tasks;

namespace Ecoba.BasePlugin.Services.PluginConfigService
{
    public interface IPluginConfigService<T, TContext>
    {
        Task<T> GetConfig();
        Task SetConfig(T config);
    }
}
