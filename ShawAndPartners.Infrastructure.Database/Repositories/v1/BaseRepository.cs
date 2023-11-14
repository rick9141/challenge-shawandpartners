using Microsoft.Data.Sqlite;
using System.Data;

namespace ShawAndPartners.Infrastructure.Database.Repositories.v1
{
    public class BaseRepository
    {
        protected readonly string _connectionString;

        public BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected IDbConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}
