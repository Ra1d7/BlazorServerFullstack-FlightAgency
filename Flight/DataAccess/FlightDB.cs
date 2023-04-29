using FlightAgency.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace FlightAgency.DataAccess
{
    public class FlightDB
    {
        public readonly IConfiguration _config;
        public FlightDB(IConfiguration config)
        {
            _config = config;
        }
        public async Task<List<User>> GetUsers()
        {
            List<User> users = new List<User>();
            using SqlConnection conn = new SqlConnection(_config.GetConnectionString("Default"));
            SqlCommand cmd = new SqlCommand("SELECT userId,username,hashedPass,roleId,email FROM Users", conn);
            await conn.OpenAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                User user = new User(
                    reader.GetInt32(0), //userid
                    reader.GetString(1), //username
                    reader.GetString(2), //password
                    reader.GetString(4), //email
                    (reader.GetInt32(3) == 1) ? Roles.Client : Roles.Admin //role
                );
                users.Add(user);
            }

            reader.Close();
            return users;
        }
        public async Task<List<FlightM>> GetFlights()
        {
            List<FlightM> flights = new List<FlightM>();
            using SqlConnection conn = new SqlConnection(_config.GetConnectionString("Default"));
            SqlCommand cmd = new SqlCommand(

                "SELECT [flightId],[from],[to],[num_of_seats],[flight_time],[cost] FROM [Flights]"

                , conn);
            await conn.OpenAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                FlightM flight = new FlightM(
                    reader.GetInt32(0), //flightid
                    reader.GetString(1), //from
                    reader.GetString(2), //to
                    reader.GetInt32(3), //seats
                    reader.GetDateTime(4), //flight_time
                    reader.GetDecimal(5) //cost
                );
                flights.Add(flight);
            }

            reader.Close();
            return flights;
        }
        public async Task<bool> LoginUser(string emailOrUsername,string pass)
        {
            bool result = false;
            var hash = Convert.ToHexString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(pass)));
            using SqlConnection conn = new SqlConnection(_config.GetConnectionString("Default"));
            SqlCommand cmd = new SqlCommand($"EXEC LoginUser {emailOrUsername},{hash}", conn);
            await conn.OpenAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result = reader.GetInt32(0) == 1 ? true : false; //check user
            }

            reader.Close();
            return result;
        }
        public async Task<User> GetUser(string term)
        {
            User user = null!;
            using SqlConnection conn = new SqlConnection(_config.GetConnectionString("Default"));
            SqlCommand cmd = new SqlCommand($"EXEC GetUser {term}", conn);
            await conn.OpenAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                user = new User(
                   reader.GetInt32(0), //userid
                   reader.GetString(1), //username
                   (reader.GetInt32(3) == 1) ? Roles.Client : Roles.Admin //role
               );
            }

            reader.Close();
            return user;
        }
    }
}
