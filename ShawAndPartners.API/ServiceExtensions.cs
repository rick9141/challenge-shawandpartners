using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ShawAndPartners.Application.Contracts.Services.v1;
using ShawAndPartners.Application.Services;
using ShawAndPartners.Domain.Contracts.Repositories;
using ShawAndPartners.Infrastructure.Configuration.Settings.v1;
using ShawAndPartners.Infrastructure.Database.Repositories.v1;

namespace ShawAndPartners.API
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInjections(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseSettings>(configuration.GetSection("AppSettings:DatabaseSettings"));


            // Add services
            services.AddScoped<IUserService, UserService>();



            // Add repositories
            services.AddScoped<IUserRepository, UserRepository>(sp =>
                            new UserRepository(sp.GetRequiredService<IOptions<DatabaseSettings>>()));

            return services;
        }

        public static void InitializeDatabase(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("AppSettings:DatabaseSettings").GetValue<string>("ConnectionString");
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            // Verifique se a tabela 'Users' existe e crie se não existir.
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
            @"
            CREATE TABLE IF NOT EXISTS Users (
                Name TEXT PRIMARY KEY,
                City TEXT,
                Country TEXT,
                FavoriteSport TEXT
            )";
            tableCmd.ExecuteNonQuery();
        }
    }
}