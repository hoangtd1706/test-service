using Ecoba.BasePlugin.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecoba.TestService.Data;
public class TestDbContext : BaseDbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }
}