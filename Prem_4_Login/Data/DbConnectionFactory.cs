using Microsoft.Data.SqlClient;
using System.Data;

namespace Prem_4_Login.API.Data
{
    public class DbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DbConnectionFactory(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString(
                    "DefaultConnection"));
        }
    }
}