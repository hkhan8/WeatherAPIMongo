using WeatherAPI.Data.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAPI.Data.Repository;
using WeatherAPI.Data.Services;
using MongoDB.Bson;

namespace WeatherAPI.Data.Repository
{
    public class UserDataRepository : IUserDataRepository
    {
        IMongoDatabase _db;
        IMongoCollection<UserData> _users;

        public UserDataRepository()
        {
            _db = MongoConnector.GetLocalDatabase("Climate");
            _users = _db.GetCollection<UserData>("Users");
        }

        public bool AuthenticateUser(string APIKey, string createAccessLevel)
        {
            // add a filter based on the APIKey
            var filter = Builders<UserData>.Filter.Eq(c => c.ApiKey, APIKey);

            // find and retrieve the user
            var user = _users.Find(filter).FirstOrDefault();

            // if the user is null or does not match the required access roles, return false
            if (user == null || (createAccessLevel != "Admin" && createAccessLevel != "Teacher" & createAccessLevel != "Student"))
            {
                // return null
                return false;
            }
            //user is set to admin all perms are granted
            else if(user.Role == "Admin")
            {
                return true;
            }
            //user is teacher, make sure only created user can be a student
            else if(user.Role == "Teacher" && createAccessLevel == "Student")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string CreateUser(UserData newUser)
        {
            var filter = Builders<UserData>.Filter.And(
                                            Builders<UserData>.Filter.Eq(c => c.Name, newUser.Name),
                                            Builders<UserData>.Filter.Eq(c => c.Email, newUser.Email));
            var existingUser = _users.Find(filter).FirstOrDefault();

            if (existingUser != null)
            {
                return "";
            }

            newUser.ApiKey = Guid.NewGuid().ToString();
            _users.InsertOne(newUser);
            return newUser.ApiKey;
        }

        public int RemoveIdleUsers(DateTime lastLogin)
        {
            throw new NotImplementedException();
        }

        public string RemoveSingleUser(string APIKey, string objid)
        {
            var filter = Builders<UserData>.Filter.Eq(c => c._id, ObjectId.Parse(objid));
            var objId = _users.DeleteOne(filter);

            return objid;
        }

        public void UpdateLoginTime(string APIKey, DateTime loginTime)
        {
            // Create a filter to find the user
            var filter = Builders<UserData>.Filter.Eq(c => c.ApiKey, APIKey);
            // Define an update to overwrite the lastaccess property
            var update = Builders<UserData>.Update.Set(c => c.LastAccess, loginTime);
            // execute the update
            _users.UpdateOne(filter, update);
        }
    }
}
