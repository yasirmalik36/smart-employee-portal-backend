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
using SmartEmpAPI.DTOs;

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

        public LoginResponse Login(LoginRequest request)
        {
            // Define stored procedure parameters
            var parameters = new[]
            {
                new SqlParameter("@Email", request.Email),
                new SqlParameter("@Password", request.Password),
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output },
                new SqlParameter("@Code", SqlDbType.NVarChar, 2) { Direction = ParameterDirection.Output },
                new SqlParameter("@Description", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output }
            };
            // Execute stored procedure and get dataset + output parameters
            var (dataSet, outputParams) = _databaseHelper.ExecuteStoredProcedurewithOutput("PRC_Employee_Login", parameters);

            // Extract output values
            string code = outputParams["@Code"]?.ToString() ?? "01";
            string message = outputParams["@Message"]?.ToString() ?? "Failure";
            string description = outputParams["@Description"]?.ToString() ?? "Invalid credentials or inactive user.";

            // Return failure response if login is invalid
            if (code != "00" || dataSet?.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            {
                return new LoginResponse { Resp = new Response { Code = code, Message = message, Description = description } };
            }

            // Retrieve user information
            var userTable = dataSet.Tables[0];
            var user = new UserLoginInfo
            {
                EmployeeID = (int)userTable.Rows[0]["EmployeeID"],
                FirstName = userTable.Rows[0]["FirstName"].ToString(),
                LastName = userTable.Rows[0]["LastName"].ToString(),
                Email = userTable.Rows[0]["Email"].ToString(),
                Gender = userTable.Rows[0]["Gender"].ToString(),
                DesignationName = userTable.Rows[0]["DesignationName"].ToString(),
                ProfileID = (int)userTable.Rows[0]["ProfileID"],
                ProfilePic = userTable.Rows[0]["ProfilePic"] as byte[]
            };

            // Retrieve roles and permissions if available
            List<Profile> roles = new List<Profile>();
            if (dataSet.Tables.Count > 1)
            {
                var rolesTable = dataSet.Tables[1];

                foreach (DataRow row in rolesTable.Rows)
                {
                    roles.Add(new Profile
                    {
                        ProfileID = (int)row["ProfileID"],
                        ProfileName = row["ProfileName"].ToString(),
                        C = Convert.ToBoolean(row["C"]),
                        R = Convert.ToBoolean(row["R"]),
                        U = Convert.ToBoolean(row["U"]),
                        D = Convert.ToBoolean(row["D"]),
                        E = Convert.ToBoolean(row["E"]),
                        Extra = Convert.ToBoolean(row["Extra"]),
                        ActivityID = (int)row["ActivityID"],
                        ActivityName = row["ActivityName"].ToString(),
                        ActivityURL = row["ActivityURL"].ToString()
                    });
                }
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            // Return success response
            return new LoginResponse
            {
                Resp = new Response { Code = "00", Message = "Success", Description = "Login Successfully" },
                Token = token,
                Act = roles
            };
        }

        private string GenerateJwtToken(UserLoginInfo user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
             new Claim("UserID", user.EmployeeID.ToString()),     
             new Claim("FirstName", user.FirstName),                           
             new Claim("LastName", user.LastName),
             new Claim("Gender", user.Gender?.ToString() ?? ""),                           
             new Claim("Designation", user.DesignationName?.ToString() ?? ""),                           
             new Claim("Email", user.Email),                      
             new Claim("ProfileID", string.Join(",", user.ProfileID))             
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
