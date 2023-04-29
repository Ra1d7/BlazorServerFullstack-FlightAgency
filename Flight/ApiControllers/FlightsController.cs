using Flight.Data;
using Flight.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Flight.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        public record CreateFlightDto(string from, string to, int seats, DateTime time, decimal cost);
        public record UpdateFlightDto(int id, string from, string to, int seats, DateTime time, decimal cost);
        private readonly IConfiguration _config;
        private readonly FlightDB _db;

        public FlightsController(IConfiguration config, FlightDB db)
        {
            _config = config;
            _db = db;
        }
        // GET: api/<FlightsController>
        [HttpGet]
        // [Authorize(Policy = "Admin")]
        public async Task<List<FlightM>> Get()
        {
            return await _db.GetFlights();
        }

        // GET api/<FlightsController>/5
        [HttpGet("{id}")]
        // [Authorize(Policy = "Admin")]
        public async Task<FlightM?> Get(int id)
        {
            return await _db.GetFlight(id);
        }

        // POST api/<FlightsController>
        [HttpPost]
        // [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Post([FromBody] CreateFlightDto flight)
        {
            if (flight != null)
            {
                FlightM toBeCreated = new FlightM(flight.from, flight.to, flight.seats, flight.time, flight.cost);
                await _db.AddFLight(toBeCreated);
                return Ok(flight);
            }
            return BadRequest();
        }

        // PUT api/<FlightsController>/5
        [HttpPut]
        // [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Put([FromBody] UpdateFlightDto flight)
        {
            if (flight != null)
            {
                try
                {
                    FlightM toBeUpdated = new(flight.id, flight.from, flight.to, flight.seats, flight.time, flight.cost);
                    if (await _db.UpdateFlight(toBeUpdated))
                    {

                        return Ok(flight);
                    }
                    return BadRequest("no users were affected");
                }
                catch (Exception e)
                {
                    return BadRequest($"An error occured while updating the user {flight.id}\n{e.Message}");
                }
            }
            return BadRequest("Could not update user");
        }

        // DELETE api/<FlightsController>/5
        [HttpDelete("{id}")]
        // [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteFlight([FromQuery]int id)
        {
            try
            {
                return Ok(await _db.DeleteFlight(id));
            }
            catch (Exception e)
            {
                return BadRequest($"Unable to delete user: {id}\n{e.Message}");
            }
        }
    }
}
