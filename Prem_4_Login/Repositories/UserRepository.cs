using Dapper;
using Prem_4_Login.API.Data;
using Prem_4_Login.API.Models;
using Prem_4_Login.API.IRepositories;
using System.Data;

namespace Prem_4_Login.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public UserRepository(
            DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        public async Task<User?> GetByLoginIdAsync(
            string loginId)
        {
            using var connection =
                _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@LoginId",
                loginId,
                DbType.String,
                size: 50);

            return await connection.QueryFirstOrDefaultAsync<User>(
                "SpGetUserByLoginId",
                parameters,
                commandType: CommandType.StoredProcedure);
        }



        public async Task<User?> GetByIdAsync(
            int userId)
        {
            using var connection =
                _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@UserId",
                userId,
                DbType.Int32);

            return await connection.QueryFirstOrDefaultAsync<User>(
                "SpGetUserById",
                parameters,
                commandType: CommandType.StoredProcedure);
        }


        public async Task<int> GetRoleIdByNameAsync(
            string roleName)
        {
            using var connection =
                _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@RoleName",
                roleName,
                DbType.String,
                size: 50);

            return await connection.ExecuteScalarAsync<int>(
                "SpGetRoleIdByName",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> CreateAsync(
            User user)
        {
            using var connection =
                _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@LoginId",
                user.LoginId);

            parameters.Add(
                "@ApplicantName",
                user.ApplicantName);

            parameters.Add(
                "@FatherName",
                user.FatherName);

            parameters.Add(
                "@MobileNumber",
                user.MobileNumber);

            parameters.Add(
                "@Email",
                user.Email);

            parameters.Add(
                "@DOB",
                user.DOB);

            parameters.Add(
                "@ProfilePic",
                user.ProfilePic);

            parameters.Add(
                "@Password",
                user.Password);

            parameters.Add(
                "@RoleId",
                user.RoleId);

            return await connection.ExecuteScalarAsync<int>(
                "SpCreateUser",
                parameters,
                commandType: CommandType.StoredProcedure);
        }


        public async Task UpdateProfileAsync(
            User user)
        {
            using var connection =
                _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@UserId",
                user.UserId);

            parameters.Add(
                "@ApplicantName",
                user.ApplicantName);

            parameters.Add(
                "@FatherName",
                user.FatherName);

            parameters.Add(
                "@Email",
                user.Email);

            parameters.Add(
                "@DOB",
                user.DOB);

            parameters.Add(
                "@ProfilePic",
                user.ProfilePic);

            await connection.ExecuteAsync(
                "SpUpdateUserProfile",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdatePasswordAsync(
            int userId,
            string password)
        {
            using var connection =
                _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@UserId",
                userId);

            parameters.Add(
                "@Password",
                password);

            await connection.ExecuteAsync(
                "SpUpdatePassword",
                parameters,
                commandType: CommandType.StoredProcedure);
        }


        public async Task<long> LogAuditAsync(
            int? userId,
            string action,
            string description,
            string? ipAddress)
        {
            using var connection =
                _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@UserId",
                userId);

            parameters.Add(
                "@Action",
                action);

            parameters.Add(
                "@Description",
                description);

            parameters.Add(
                "@IpAddress",
                ipAddress);

            return await connection.ExecuteScalarAsync<long>(
                "SpInsertAuditTrail",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<(IEnumerable<User> Users, int TotalRecords)>
            GetUsersAsync(
                int pageNumber,
                int pageSize,
                string? search)
        {
            using var connection =
                _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add(
                "@PageNumber",
                pageNumber);

            parameters.Add(
                "@PageSize",
                pageSize);

            parameters.Add(
                "@Search",
                search);

            using var multi =
                await connection.QueryMultipleAsync(
                    "SpGetUsers",
                    parameters,
                    commandType:
                        CommandType.StoredProcedure);

            var totalRecords =
                await multi.ReadFirstAsync<int>();

            var users =
                await multi.ReadAsync<User>();

            return (
                users,
                totalRecords
            );
        }
    }
}