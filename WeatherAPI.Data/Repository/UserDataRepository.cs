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
            var result = _users.DeleteOne(filter);

            return objid;
        }

        public string RemoveMultipleUsers(string APIKey, string objid)
        {
            //split entered ids with ',' in input and no spaces between 
            var ids = objid.Split(',').ToList();
            List<ObjectId> idparse = new List<ObjectId>();
            foreach(var item in ids)
            {
                //after separation of ids, parse each item into objectid
                idparse.Add(ObjectId.Parse(item));
            }
            //results enumerate on idparse
            var eachResult = idparse.AsEnumerable();

            var filter = Builders<UserData>.Filter.In(c => c._id, eachResult);
            //delete all entered inputs in id split with ','
            var objId = _users.DeleteMany(filter);

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

        public bool UpdateRoles(string userRoles)
        {
            //split entered ids with ',' in input and no spaces between 
            var ids = userRoles.Split(',').ToList();
            List<ObjectId> idparse = new List<ObjectId>();

            for (int i = 0; i < ids.Count; i += 2)
            {
                //first index of string is for id
                var item = ids[i];
                //second index is role
                var role = ids[i + 1];

                idparse.Add(ObjectId.Parse(item));
                // Create a filter to find the user
                var filter = Builders<UserData>.Filter.Eq(c => c._id, ObjectId.Parse(item));
                // Define an update to overwrite property
                var update = Builders<UserData>.Update.Set(c => c.Role, role);
                _users.UpdateOne(filter, update);
            }
            
            return false;
        }
    }
}
