using System.Text;
using Ecoba.IdentityService.Services.UserService;
using Ecoba.IdentityService.Services.Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ServiceDiscovery.Consul;
using Ecoba.TestService.Data;
using Ecoba.BasePlugin.Services.ModeratorService;
using Ecoba.BasePlugin.Services.PluginConfigService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TestDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped(typeof(IModeratorService<>), typeof(ModeratorService<>));
builder.Services.AddScoped(typeof(IPluginConfigService<,>), typeof(PluginConfigService<,>));

builder.Services.AddScoped<IConsulService, ConsulService>();
// Add services to the container.
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
 {
     options.RequireHttpsMetadata = false;
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
         ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
         IssuerSigningKeys = new[]{
             new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value)),
             new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:KeyService").Value)),
         },
     };
 });

var serverConfig = builder.Configuration.GetServiceConfig();
builder.Services.AddConsul(serverConfig);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

void InitializeDatabase(IApplicationBuilder app)
{
    var serviceScopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
    if (serviceScopeFactory != null)
    {
        using var scope = serviceScopeFactory.CreateScope();
        scope.ServiceProvider.GetRequiredService<TestDbContext>().Database.Migrate();
    }
}