using ShawAndPartners.Infrastructure.Database.Repositories.v1;
using System.Data;
using Xunit;

namespace ShawAndPartners.Tests.InfrastructureTests.RepositoriesTests.v1
{
    /// <summary>
    /// Integration tests using the AAA standard.
    /// </summary>
    public class BaseRepositoryIntegrationTests
    {
        [Fact]
        public void CreateConnection_ShouldOpenNewConnection()
        {
            var connectionString = "Data Source=:memory:";
            var baseRepository = new TestableBaseRepository(connectionString);

            using var connection = baseRepository.TestCreateConnection();

            Assert.NotNull(connection);
            Assert.Equal(ConnectionState.Closed, connection.State); 

            connection.Open();
            Assert.Equal(ConnectionState.Open, connection.State);
        }
    }

    public class TestableBaseRepository : BaseRepository
    {
        public TestableBaseRepository(string connectionString) : base(connectionString)
        {
        }

        public IDbConnection TestCreateConnection()
        {
            return base.CreateConnection();
        }
    }

}
