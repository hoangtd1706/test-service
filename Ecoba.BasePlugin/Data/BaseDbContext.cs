using Ecoba.BasePlugin.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecoba.BasePlugin.Data
{
    public abstract class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Moderator> Moderators { get; set; }
        public DbSet<PluginConfig> PluginConfigs { get; set; }
    }
}
