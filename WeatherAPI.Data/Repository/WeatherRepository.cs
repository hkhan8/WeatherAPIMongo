using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using WeatherAPI.Data.Models;
using WeatherAPI.Data.Services;


namespace WeatherAPI.Data.Repository
{
    public class WeatherRepository
    {
        IMongoCollection<Weather> _collection;
        FilterDefinitionBuilder<Weather> _builder;

        public WeatherRepository(string connectionstring = "")
        {
            IMongoDatabase db;
            if (!String.IsNullOrEmpty(connectionstring))
            {
                db = MongoConnector.GetDatabase(connectionstring, "Climate");
            }
            else
            {
                db = MongoConnector.GetLocalDatabase("Climate");
            }

            // Added to allow for using the filter throughout the repository
            _builder = Builders<Weather>.Filter;

            _collection = db.GetCollection<Weather>("WeatherStation");
        }

        // get single reading
        public Weather GetSingleReadingByID(string objId)
        {
            var filter = _builder.Eq(c => c._id, ObjectId.Parse(objId));
            var filer2 = _builder.Eq(c => c._ObjId, objId);

            return _collection.Find(filter).FirstOrDefault();
        }

        // Get All readings
        //public List<Weather> GetAll()
        //{
        //    return _collection.Find(_builder.Empty).ToList();
        //}

        // insert new weather readings
        public void InsertWeatherReading(Weather weather)
        {
            _collection.InsertOne(weather);
        }

        public void UpdateLongLat(string objid, decimal newLongitude, decimal newLatitude)
        {
            // perform the update on the specified object
            var filter = _builder.Eq(c => c._id, ObjectId.Parse(objid));

            // specify the property to be updated, and the value to update it with
            var update1 = Builders<Weather>.Update.Set(c => c.Longitude, newLongitude);
            var update2 = Builders<Weather>.Update.Set(c => c.Latitude, newLatitude);

            // Update values based on the ID
            var result1 = _collection.UpdateOne(filter, update1);
            var result2 = _collection.UpdateOne(filter, update2);
        }

        public MaxPrecipitation MaxPrecipitation()
        {
            var timeLatest = _collection.Find(x => true).SortByDescending(d => d.Time).Limit(1).FirstOrDefault();
            //var time_5months_before = time_now.AddMonths(-15);
            timeLatest.Time = timeLatest.Time.AddMonths(-5);
            var test = _collection.Find(x => x.Time >= timeLatest.Time).SortByDescending(d => d.Precipitation).Limit(1).FirstOrDefault();
            //instantiate new class holding only the percipitation value
            MaxPrecipitation max = new MaxPrecipitation();
            max.Precipitation = test.Precipitation;
            return max;
        }

        public void InsertFarenheightFields()
        {




        }


        public List<ReadingsByDate> ReadingsByDate(DateTime min, DateTime max)
        {
            //setting min to zero for easier input
            min = min.AddMilliseconds(min.Millisecond * (-1));
            min = min.AddSeconds(min.Second * (-1));
            min = min.AddMinutes(min.Minute * (-1));
            //setting max to zero for easier input
            max = max.AddMilliseconds(max.Millisecond * (-1));
            max = max.AddSeconds(max.Second * (-1));
            max = max.AddMinutes(max.Minute * (-1));

            //linq query to select entries between 2 dates
            var results = _collection.AsQueryable()
            .Where(p => p.Time > min && p.Time < max)
            .Select(p => new { p.AtmoPressure, p.Temperature, p.Precipitation, p.Radiation});

            var a = results.GetEnumerator();

            List<ReadingsByDate> readingsByDates = new List<ReadingsByDate>();
            while (a.MoveNext())
            {      
                //instiate class to hold only required properties
                ReadingsByDate reading = new ReadingsByDate();
                //set properties to class
                reading.AtmoPressure = a.Current.AtmoPressure;
                reading.Temperature = a.Current.Temperature;
                reading.Precipitation = a.Current.Precipitation;
                reading.Radiation = a.Current.Radiation;
                //add reading to list              
                readingsByDates.Add(reading);
            }
            return readingsByDates;
        }
          
    }
}