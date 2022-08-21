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
            if (!IsAuthenticated(APIKey, newUser.Role))
            {
                return Unauthorized();
            }

            var userAPIKey = _users.CreateUser(newUser);

            return Ok(userAPIKey);
        }

        private bool IsAuthenticated(string APIKey, string createdAccessLevel)
        {
            _users.UpdateLoginTime(APIKey, DateTime.Now);
            return _users.AuthenticateUser(APIKey, createdAccessLevel);            
        }

        [HttpDelete("DeleteUserByID")]
        public IActionResult RemoveSingleUser(string APIKey, string objid)
        {
            if(!IsAuthenticated(APIKey, "Admin"))
            {
                return Unauthorized();
            }
            var deleteUser = _users.RemoveSingleUser(APIKey, objid);
            return Ok(deleteUser);
        }

    }
}
