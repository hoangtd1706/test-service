using Ecoba.BasePlugin.Data;
using Ecoba.BasePlugin.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Ecoba.BasePlugin.Services.PluginConfigService
{
    public class PluginConfigService<T, TContext> : IPluginConfigService<T, TContext> where T : class where TContext : BaseDbContext
    {
        private readonly TContext _context;
        private readonly ILogger<PluginConfigService<T, TContext>> _logger;
        public PluginConfigService(TContext context, ILogger<PluginConfigService<T, TContext>> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<T> GetConfig()
        {
            var key = typeof(T).Name;
            var config = await _context.PluginConfigs.FirstOrDefaultAsync(c => c.Key == key);
            if (config == null)
                return null;

            try
            {
                var result = JsonConvert.DeserializeObject<T>(config.Json);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on deserialize object");
            }

            return null;
        }

        public async Task SetConfig(T config)
        {
            var key = typeof(T).Name;
            var exist = await _context.PluginConfigs.FirstOrDefaultAsync(c => c.Key == key);
            if (exist == null)
            {
                try
                {
                    _context.PluginConfigs.Add(new PluginConfig()
                    {
                        Key = key,
                        Json = JsonConvert.SerializeObject(config)
                    });
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error on add new config");
                }
            }
            else
            {
                try
                {
                    exist.Json = JsonConvert.SerializeObject(config);
                    _context.PluginConfigs.Update(exist);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error on update config");
                }
            }
        }
    }
}
