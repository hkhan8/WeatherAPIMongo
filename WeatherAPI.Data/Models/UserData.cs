using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAPI.Data.Models
{
    public class UserData
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string ApiKey { get; set; }
        public DateTime LastAccess { get; set; } = DateTime.Now;
        public string Role { get; set; } = "Student";
    }
}
