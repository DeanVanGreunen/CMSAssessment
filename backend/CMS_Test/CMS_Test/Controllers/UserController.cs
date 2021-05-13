using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CMS_Test.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CMS_Test.CModels;

namespace CMS_Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController :  ControllerBase

    {        
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;

        public UserController(ILogger<UserController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _connection = new SqlConnection(_configuration["Database:ConnectionString"]);
        }

        // CORS BUG FIX
        [HttpOptions]
        [Route("/api/user/register")]
        [Route("/api/user/login")]
        [Route("/api/user/logout")]
        
        public HttpResponseMessage Options()
        {
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

        [HttpPost]
        [Route("/api/user/register")]
        
        public UserSession Register([FromBody] RegisterModel register) // string email, string password, string confirm_password
        {
            if (register.Password != register.ConfirmPassword)
            {
                return new UserSession()
                {
                    Success = false,
                    Message = "Error Passwords don't match",
                    SessionKey = "",
                };
            }

            _connection.Open();
            SqlCommand select_command = new SqlCommand("Select id from [dbo].[user] where email LIKE @email", _connection);
            select_command.Parameters.AddWithValue("@email", register.Email);
            using (SqlDataReader select_reader = select_command.ExecuteReader())
            {
                if (select_reader.HasRows)
                {
                    _connection.Close();
                    return new UserSession()
                    {
                        Success = false,
                        Message = "Error user already exists",
                        SessionKey = "",
                    };
                }
            }

            // insert
            string hashed_password = Helper.Hash(register.Password);
            string session_key = Helper.RandomString(32);

            SqlCommand insert_command = new SqlCommand("INSERT INTO [dbo].[user]([email],[password_hash],[session_key]) VALUES (@email,@passwordhash,@sessionkey)", _connection);
            insert_command.Parameters.AddWithValue("@email", register.Email);
            insert_command.Parameters.AddWithValue("@passwordhash", hashed_password);
            insert_command.Parameters.AddWithValue("@sessionkey", session_key);
            insert_command.CommandType = CommandType.Text;
            insert_command.ExecuteNonQuery();

            _connection.Close();
            
            return new UserSession()
            {
                Success = true,
                Message = "User has been registered",
                SessionKey = session_key, // TODO GET SESSIONKEY
            };
        }

        [HttpPost, Route("/api/user/login")]
        
        public UserSession Login([FromBody] LoginModel login)
        {
            // default deny-allow pattern
            UserSession usersession = new UserSession()
            {
                Success = false,
                Message = "Could not sign in user",
                SessionKey = "",
            };

            if (login.Email == null || login.Email.Length <= 0)
            {
                return new UserSession()
                {
                    Success = false,
                    Message = "Invalid Email",
                    SessionKey = "",
                };
            }
            if (login.Password == null || login.Password.Length <= 0)
            {
                return new UserSession()
                {
                    Success = false,
                    Message = "Invalid Password",
                    SessionKey = "",
                };
            }


            string hashed_password = Helper.Hash(login.Password);
            string session_key = Helper.RandomString(32);

            _connection.Open();
            SqlCommand select_command = new SqlCommand("Select id, email, session_key from [user] where email like @email and password_hash like @passwordhash", _connection);
            select_command.Parameters.AddWithValue("@email", login.Email);
            select_command.Parameters.AddWithValue("@passwordhash", hashed_password);

            using (SqlDataReader select_reader = select_command.ExecuteReader())
            {
                if (select_reader.Read())
                {
                    if (select_reader.HasRows)
                    {
                        // override default usersession variable if login is successfuly
                        usersession.Success = true;
                        usersession.Message = "User logged in successfully";
                        usersession.SessionKey = session_key;
                    }
                }
            }

            SqlCommand update_command = new SqlCommand("UPDATE [dbo].[user] SET [session_key] = @sessionkey WHERE email like @email", _connection);
            update_command.Parameters.AddWithValue("@email", login.Email);
            update_command.Parameters.AddWithValue("@sessionkey", session_key);
            update_command.CommandType = CommandType.Text;
            update_command.ExecuteNonQuery();


            _connection.Close();
            return usersession;
        }

        [HttpPost]
        [Route("/api/user/logout")]
        
        public UserSession Logout([FromBody] LogoutModel logout)
        {;
            // get user based on email and password
            if(logout.SessionKey.Length <= 0)
            {
                return new UserSession()
                {
                    Success = false,
                    Message = "Invalid Session Key",
                    SessionKey = ""
                };
            }

            _connection.Open();

            SqlCommand update_command = new SqlCommand("UPDATE [dbo].[user] SET [session_key] = '' WHERE [session_key] like @session_key", _connection);
            update_command.Parameters.AddWithValue("@session_key", logout.SessionKey);
            update_command.CommandType = CommandType.Text;
            update_command.ExecuteNonQuery();

            _connection.Close();

            return new UserSession() { 
                Success = true,
                Message = "User logged out succesfully",
                SessionKey = ""
            };
        }
    }
}
