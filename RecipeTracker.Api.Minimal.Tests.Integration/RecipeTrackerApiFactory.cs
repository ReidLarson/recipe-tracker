using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RecipeTracker.Core.Data;

namespace RecipeTracker.Api.Minimal.Tests.Integration;

public class RecipeTrackerApiFactory : WebApplicationFactory<IMarker>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(IDbConnectionFactory));
            services.AddSingleton<IDbConnectionFactory>(_ =>
                new SqliteConnectionFactory("DataSource=InMemory;Mode=Memory&Cache=Shared"));
        });
    }
}