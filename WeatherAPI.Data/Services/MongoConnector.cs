using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace WeatherAPI.Data.Services
{
    public static class MongoConnector
    {
        public static IMongoDatabase GetLocalDatabase(string databaseName)
        {
            var client = new MongoClient("mongodb://localhost:27017/");
            return client.GetDatabase(databaseName);
        }

        public static IMongoDatabase GetDatabase(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            return client.GetDatabase(databaseName);
        }

        public static IMongoDatabase GetAtlasDatabase(string username, string password, string server,
            string connection, string databaseName)
        {
            var connectionString = $"{server}{username}:{password}{connection}";
            var client = new MongoClient(connectionString);
            return client.GetDatabase(databaseName);
        }
    }
}
