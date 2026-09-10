using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace Ecommerce.Tests.Integration.Factory
{
    public class DockerWebApplicationFactoryFixture : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer;
        private NpgsqlConnection _dbConnection = null!;
        private Respawner _respawner = null!;
        public DockerWebApplicationFactoryFixture()
        {
            _dbContainer = new PostgreSqlBuilder("postgres:16").Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(x =>
            {
                x.RemoveAll<AppDbContext>();
                x.RemoveAll<DbContextOptions<AppDbContext>>();
                x.AddDbContext<AppDbContext>(options =>
                {
                    options.UseNpgsql(_dbContainer.GetConnectionString());
                });
            });
        }
        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            using (var scope = Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await dbContext.Database.EnsureCreatedAsync();
                _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());

                await _dbConnection.OpenAsync();
                _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions()
                {
                    DbAdapter = DbAdapter.Postgres
                });
            }
        }
        public async Task ResetRespawner()
        {
            await _respawner.ResetAsync(_dbConnection);
        }
        async Task IAsyncLifetime.DisposeAsync()
        {
            await _dbContainer.DisposeAsync();
        }
    }
}
