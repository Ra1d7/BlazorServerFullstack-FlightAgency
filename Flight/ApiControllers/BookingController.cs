using Flight.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Flight.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        public record BookingDetailsDto(int userid,int flightid);
        public record BookingDetails(int userid,int flightid,DateTime time);
        private readonly IConfiguration _config;
        private readonly FlightDB _db;

        public BookingController(IConfiguration config, FlightDB db)
        {
            _config = config;
            _db = db;
        }
        [HttpGet]
        public async Task<List<BookingDetails>> GetBookings()
        {
            return await _db.GetBookings();
        }
        [HttpGet("{userid}")]
        public async Task<BookingDetails?> GetBooking(int userid)
        {
            return await _db.GetBooking(userid);
        }
        [HttpPost]
        public async Task<IActionResult> BookFlight([FromBody] BookingDetails bookingDetails)
        {
            try
            {
                bool result = await _db.BookFlight(bookingDetails.userid, bookingDetails.flightid);
                if (result) return Ok("Flight Booked!");
                return BadRequest($"Couldn't book flight for user {bookingDetails.userid} at flight {bookingDetails.flightid}");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error has occured while trying to book the flight {ex.Message}");
            }
        }
    }
}
