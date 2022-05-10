using System.Security.Claims;
using System.Net;
using System.Collections.Generic;
namespace Ecoba.IdentityService.Services.UserService;
using Consul;
using Ecoba.IdentityService.Data.Model;
using Microsoft.Extensions.Configuration;
using Ecoba.IdentityService.Common.Contants;
using System.Collections;
using Microsoft.AspNetCore.Http;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly IConsulService _consulService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly HttpContext _httpContext;

    public UserService(IConsulService consulService, IConfiguration config, HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _consulService = consulService;
        _config = config;
        _httpClient = httpClient;
        _httpContext = httpContextAccessor.HttpContext;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        var result = await _consulService.GetAsync<IEnumerable<User>>(ServiceContant.IDENTITY_SERVICE, "api/v1/users/user", true);
        return result;
    }

    public bool CheckAdmin()
    {
        var userId = _httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        string[] adminIds = { "000", "-1000", "-1001" };
        if (adminIds.Contains(userId)) return true;
        return false;
    }
}