using Dapper;
using Microsoft.Extensions.Options;
using ShawAndPartners.Domain.Contracts.Repositories;
using ShawAndPartners.Domain.Entities.v1;
using ShawAndPartners.Infrastructure.Configuration.Settings.v1;

namespace ShawAndPartners.Infrastructure.Database.Repositories.v1
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly DatabaseSettings _databaseSettings;

        public UserRepository(IOptions<DatabaseSettings> databaseSettings)
            : base(databaseSettings.Value.ConnectionString)
        {
            _databaseSettings = databaseSettings.Value;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var query = "SELECT * FROM Users";
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<User>(query);
            }
        }

        public async Task<User> GetByNameAsync(string name)
        {
            var query = "SELECT * FROM Users WHERE Name = @Name";
            using (var connection = CreateConnection())
            {
                return (await connection.QueryAsync<User>(query, new { Name = name })).FirstOrDefault();
            }
        }

        public async Task<int> InsertAsync(User user)
        {
            var query = "INSERT INTO Users (Name, City, Country, FavoriteSport) VALUES (@Name, @City, @Country, @FavoriteSport)";
            using (var connection = CreateConnection())
            {
                return await connection.ExecuteAsync(query, user);
            }
        }

        public async Task<int> UpdateAsync(User user)
        {
            var query = "UPDATE Users SET City = @City, Country = @Country, FavoriteSport = @FavoriteSport WHERE Name = @Name";
            using (var connection = CreateConnection())
            {
                return await connection.ExecuteAsync(query, user);
            }
        }

        public async Task<int> DeleteByNameAsync(string name)
        {
            var query = "DELETE FROM Users WHERE Name = @Name";
            using (var connection = CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { Name = name });
            }
        }

        public async Task<IEnumerable<User>> SearchAsync(string searchTerm)
        {
            var query = @"SELECT * FROM Users WHERE 
                      Name LIKE @SearchTerm OR 
                      City LIKE @SearchTerm OR 
                      Country LIKE @SearchTerm OR 
                      FavoriteSport LIKE @SearchTerm";

            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<User>(query, new { SearchTerm = $"%{searchTerm}%" });
            }
        }
    }
}
