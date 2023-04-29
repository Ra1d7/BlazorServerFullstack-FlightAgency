using Flight.Data;
using Flight.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Flight.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public record UpdateUserDto(int userid,string username,string password,string email);
        public record CreateUserDto(string username, string password, string email);
        private readonly IConfiguration _config;
        private readonly FlightDB _db;

        public UsersController(IConfiguration config,FlightDB db) 
        {
            _config = config;
            _db = db;
        }
        // GET: api/<UsersController>
        [HttpGet]
        // [Authorize(Policy = "Admin")]
        public async Task<List<User>> Get()
        {
            return await _db.GetUsers();
        }

        // GET api/<UsersController>/5
        [HttpGet("{emailOrUsername}")]
        // [Authorize(Policy = "Admin")]
        public async Task<User?> Get(string emailOrUsername)
        {
            return await _db.GetUser(emailOrUsername);
        }

        // POST api/<UsersController>
        [HttpPost]
        // [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Post([FromBody] CreateUserDto user)
        {
           if (user != null) 
            {
                User toBeCreated = new User(user.username, user.password, user.email);
                await _db.CreateUser(toBeCreated);
                return Ok(user);
            }
            return BadRequest();
        }

        // PUT api/<UsersController>/5
        [HttpPut]
        // [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Put([FromBody] UpdateUserDto user)
        {
            if(user != null)
            {
                try
                {
                    User toBeUpdated = new User(user.userid,user.username,user.password,user.email);
                    if(await _db.UpdateUser(toBeUpdated))
                    {

                    return Ok(user);
                    }
                    return BadRequest("no users were affected");
                }
                catch(Exception e)
                {
                    return BadRequest($"An error occured while updating the user {user.userid}\n{e.Message}");
                }
            }
            return BadRequest("Could not update user");
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        // [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                return Ok(await _db.DeleteUser(id));
            }
            catch(Exception e)
            {
                return BadRequest($"Unable to delete user: {id}\n{e.Message}");
            }
        }
    }
}
