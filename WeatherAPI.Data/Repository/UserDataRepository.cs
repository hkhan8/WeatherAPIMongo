using WeatherAPI.Data.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAPI.Data.Repository;
using WeatherAPI.Data.Services;

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

        public UserData AuthenticateUser(string APIKey, string requiredAccess)
        {
            // add a filter based on the APIKey
            var filter = Builders<UserData>.Filter.Eq(c => c.ApiKey, APIKey);

            // find and retrieve the user
            var user = _users.Find(filter).FirstOrDefault();

            // if the user is null or does not match the required access
            if (user == null || !user.Role.Equals(requiredAccess))
            {
                // return null
                return null;
            }
            // return the user
            return user;
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

        public void RemoveSingleUser(string APIKey)
        {
            throw new NotImplementedException();
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
