using WeatherAPI.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherAPI.Data.Repository;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private readonly IUserDataRepository _users;
        public UserDataController(IUserDataRepository users)
        {
            _users = users;
        }

        // Create a new User
        [HttpPost("CreateNewUser")]
        public IActionResult CreateUser(string APIKey, UserData newUser)
        {
            if (!IsAuthenticated(APIKey, "Admin"))
            {
                return Unauthorized();
            }

            var userAPIKey = _users.CreateUser(newUser);

            return Ok(userAPIKey);
        }

        private bool IsAuthenticated(string APIKey, string requiredAccess)
        {
            if (_users.AuthenticateUser(APIKey, requiredAccess) == null)
            {
                return false;
            }
            _users.UpdateLoginTime(APIKey, DateTime.Now);
            return true;
        }


    }
}
