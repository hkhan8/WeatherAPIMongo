using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Data.Models;
using WeatherAPI.Data.Repository;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        WeatherRepository _weatherRepo;
        public WeatherController()
        {
            _weatherRepo = new WeatherRepository();
        }

        //[HttpGet("GetAllWeatherReadings")]
        //public IActionResult GetAllReadings()
        //{
        //    return Ok(_weatherRepo.GetAll());
        //}

        [HttpGet("GetSingleWeatherReadingByID")]
        public IActionResult GetSingleReadingByID(string objid)
        {
            return Ok(_weatherRepo.GetSingleReadingByID(objid));
        }

        [HttpGet("GetMaxPrecipitationIn5Months")]
        public IActionResult Max()
        {
            return Ok(_weatherRepo.MaxPrecipitation());
        }

        [HttpGet("GetReadingsByDate")]
        public IActionResult GetReadingsByDate(DateTime older_date, DateTime latest_date)
        {
            return Ok(_weatherRepo.ReadingsByDate(older_date, latest_date));
        }

        [HttpPost("InsertNewWeatherReadings")]
        public IActionResult CreateWeatherReadings(Weather weather)
        {
            _weatherRepo.InsertWeatherReading(weather);
            return CreatedAtAction("CreateWeatherReadings", weather);
        }

        //[HttpPost("InsertFarenheight")]
        //public IActionResult InsertFarenheight()
        //{
        //    "Temperature(F)": { $add: [ { $multiply: ['$Temperature (°C)', 1.8] }, 32]}            
        //}

        [HttpPut("UpdateLong&Lat")]
        public IActionResult UpdateLocation(string objid, decimal newLongitude, decimal newLatitude)
        {
            _weatherRepo.UpdateLongLat(objid, newLongitude, newLatitude);

            return NoContent();
        }

        
    }
}
