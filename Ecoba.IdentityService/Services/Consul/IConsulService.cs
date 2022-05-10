namespace Ecoba.IdentityService.Services.Consul;

public interface IConsulService
{
    Task<T> GetAsync<T>(string serviceName, string requestUri, bool withToken = false);
}