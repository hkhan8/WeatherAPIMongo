using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherAPI.Data.Models
{
    //admin apikey: d338a756-ba33-4155-9133-ff08673aa247
    //teacher apikey: f25ee46b-3107-420f-abe3-06341782bf7a
    //student apikey: 81fbf43d-4a0f-4916-bc0c-5a66ca265fd9
    public class UserData
    {
        [JsonIgnore]
        [BsonId]
        public ObjectId _id { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        [JsonIgnore]
        public string? ApiKey { get; set; }
        [JsonIgnore]
        public DateTime LastAccess { get; set; } = DateTime.Now;
        [DefaultValue("Student")]
        public string Role { get; set; } = "Student";
    }
}
