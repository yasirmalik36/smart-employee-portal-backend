using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartEmpAPI.DAL;
using SmartEmpAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Data;
using SmartEmpAPI.Interfaces;

namespace SmartEmpAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly DatabaseHelper _databaseHelper;
        private readonly IConfiguration _configuration;

        public AuthService(DatabaseHelper databaseHelper, IConfiguration configuration)
        {
            _databaseHelper = databaseHelper;
            _configuration = configuration;
        }

        public LoginResponse Login(string email, string passwordHash)
        {
            var parameters = new[]
            {
                new SqlParameter("@Identifier", email),
                new SqlParameter("@PasswordHash", passwordHash)
            };

            var dataSet = _databaseHelper.ExecuteStoredProcedure("sp_UserLogin", parameters);


            // Validate if dataset is null or contains no tables
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            {
                return new LoginResponse
                {
                    StatusCode = "01",
                    StatusMessage = "Invalid email or password, or user is inactive."
                };
            }

            var userTable = dataSet.Tables[0];

            // Check if the first row contains an error message
            if (userTable.Columns.Contains("ErrorMessage") && userTable.Rows.Count > 0)
            {
                return new LoginResponse
                {
                    StatusCode = userTable.Rows[0]["StatusCode"].ToString(),
                    StatusMessage = userTable.Rows[0]["ErrorMessage"].ToString()
                };
            }
            var rolesTable = dataSet.Tables[1];

            var user = new User
            {
                UserID = (int)userTable.Rows[0]["UserID"],
                Username = userTable.Rows[0]["Username"].ToString(),
                FirstName = userTable.Rows[0]["FirstName"].ToString(),
                LastName = userTable.Rows[0]["LastName"].ToString(),
                Gender = userTable.Rows[0]["Gender"].ToString(),
                Role = userTable.Rows[0]["Role"].ToString(),
                Email = userTable.Rows[0]["Email"].ToString(),
                IsActive = (bool)userTable.Rows[0]["IsActive"],
                LastLogin = userTable.Rows[0]["LastLogin"] as DateTime?,
                IPAddress = userTable.Rows[0]["IPAddress"].ToString()
            };

            var roles = new List<Role>();
            foreach (DataRow row in rolesTable.Rows)
            {
                roles.Add(new Role
                {
                    RoleID = (int)row["RoleID"],
                    RoleName = row["RoleName"].ToString(),
                    C = (bool)row["C"],
                    R = (bool)row["R"],
                    U = (bool)row["U"],
                    D = (bool)row["D"],
                    E = (bool)row["E"],
                    Extra = (bool)row["Extra"],
                    MappingID = (int)row["MappingID"],
                    ActivityID = (int)row["ActivityID"],
                    ActivityName = row["ActivityName"].ToString(),
                    ActivityURL = row["ActivityURL"].ToString()
                });
            }

             user.Token= GenerateJwtToken(user);
            return new LoginResponse
            {
                User = user,
                Roles = roles,
                StatusCode="00",
                StatusMessage="Success"
            };
        }
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
             new Claim("UserID", user.UserID.ToString()),     
             new Claim("Username", user.Username),                           
             new Claim("FirstName", user.FirstName),                           
             new Claim("LastName", user.LastName),                           
             new Claim("Gender", user.Gender),                           
             new Claim("Email", user.Email),                      
             new Claim("role", string.Join(",", user.Role))             
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow, // nbf: Token is not valid before this time
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiryInMinutes"])), // exp: Token expiration time
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
