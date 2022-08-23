using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeatherAPI.Data.Models
{
    public class Weather
    {
        [BsonId]
        public ObjectId _id { get; set; }

        //private string _objId;

        public virtual string _ObjId
        {
            get { return _id.ToString(); }
        }
        public DateTime Time { get; set; }
        //stores it as decimal in update.set method for updating long,lat 
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Latitude { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Longitude { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Location { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [BsonElement("Temperature (°C)")]
        public decimal Temperature { get; set; }
        [BsonElement("Atmospheric Pressure (kPa)")]
        public decimal AtmoPressure { get; set; }
        [BsonElement("Precipitation mm/h")]
        public decimal Precipitation { get; set; }
        [BsonElement("Solar Radiation (W/m2)")]
        public decimal Radiation { get; set; }
    }

    //class to display specific properties
    public class ReadingsByDate
    {
        [BsonElement("Temperature (°C)")]
        public decimal Temperature { get; set; }
        [BsonElement("Atmospheric Pressure (kPa)")]
        public decimal AtmoPressure { get; set; }
        [BsonElement("Precipitation mm/h")]
        public decimal Precipitation { get; set; }
        [BsonElement("Solar Radiation (W/m2)")]
        public decimal Radiation { get; set; }
    }

    //class to display specific property
    public class MaxPrecipitation
    {
        [BsonElement("Precipitation mm/h")]
        public decimal Precipitation { get; set; }
    }
}