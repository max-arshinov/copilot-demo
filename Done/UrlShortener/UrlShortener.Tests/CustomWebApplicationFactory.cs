using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using UrlShortener.ReadApi.Controllers;
using UrlShortener.ReadApi.Services;

namespace UrlShortener.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<UrlController>
{
    public Mock<IUrlCache> MockUrlCache { get; } = new();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing UrlCache registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(UrlCache));
            
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            
            // Add the mock UrlCache
            services.AddSingleton(MockUrlCache.Object);
        });
    }
}
