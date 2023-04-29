using FlightAgency.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using static Flight.ApiControllers.BookingController;

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
                var role = reader.GetInt32(3);
                User user = new User(
                    reader.GetInt32(0), //userid
                    reader.GetString(1), //username
                    reader.GetString(2), //password
                    reader.GetString(4), //email
                    (role == 1) ? Roles.Client : Roles.Admin //role
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
            SqlCommand cmd = new SqlCommand($"EXEC LoginUser @emailOrUsername,@hash", conn);
            cmd.Parameters.AddWithValue("@emailOrUsername", emailOrUsername);
            cmd.Parameters.AddWithValue("@hash", hash);
            await conn.OpenAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result = reader.GetInt32(0) == 1 ? true : false; //check user
            }

            reader.Close();
            return result;
        }
        public async Task<User?> GetUser(string term)
        {
            User user = null!;
            try
            {
            using SqlConnection conn = new SqlConnection(_config.GetConnectionString("Default"));
            SqlCommand cmd = new SqlCommand("EXEC GetUser @term", conn);
            cmd.Parameters.AddWithValue("@term", term);
            await conn.OpenAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                user = new User(
                   reader.GetInt32(0), //userid
                   reader.GetString(1), //username
                   (reader.GetInt32(2) == 1) ? Roles.Client : Roles.Admin //role
               );
            }

            reader.Close();
            }
            catch
            {
                return null;
            }
            return user;
        }
        public async Task<bool> CreateUser(User user)
        {
            try
            {
            var hashedpassword = Convert.ToHexString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(user.Password)));
            using SqlConnection conn = new SqlConnection(_config.GetConnectionString("Default"));
            SqlCommand cmd = new SqlCommand("EXEC Admin_CreateUser @username,@password,@role,@email", conn);
            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@password", hashedpassword);
            cmd.Parameters.AddWithValue("@role", user.Role == Roles.Client ? 1 : 2);
            cmd.Parameters.AddWithValue("@email", user.Email);
            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() == 0 ? false : true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateUser(User user)
        {
            try
            {
            var hashedpassword = Convert.ToHexString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(user.Password)));
            using SqlConnection conn = new SqlConnection(_config.GetConnectionString("Default"));
            SqlCommand cmd = new SqlCommand("EXEC Admin_UpdateUser @userid,@username,@password,@role,@email", conn);
            cmd.Parameters.AddWithValue("@userid", user.userId);
            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@password", hashedpassword);
            cmd.Parameters.AddWithValue("@role", user.Role == Roles.Client ? 1 : 2);
            cmd.Parameters.AddWithValue("@email", user.Email);
            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() == 0 ? false : true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteUser(int id)
        {
            try
            {
            using SqlConnection conn = new SqlConnection(_config.GetConnectionString("Default"));
            SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE userId = @userid", conn);
            cmd.Parameters.AddWithValue("@userid", id);
            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() == 0 ? false : true;
            }
            catch { return false; }
        }
        public async Task<FlightM?> GetFlight(int id)
        {
            FlightM flight = null;
            using SqlConnection conn = new SqlConnection(_config.GetConnectionString("Default"));
            SqlCommand cmd = new SqlCommand(

                "SELECT [flightId],[from],[to],[num_of_seats],[flight_time],[cost] FROM [Flights] WHERE flightId = @id"

                , conn);
            cmd.Parameters.AddWithValue("@id", id);
            await conn.OpenAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                flight = new FlightM(
                    reader.GetInt32(0), //flightid
                    reader.GetString(1), //from
                    reader.GetString(2), //to
                    reader.GetInt32(3), //seats
                    reader.GetDateTime(4), //flight_time
                    reader.GetDecimal(5) //cost
                );
            }

            reader.Close();
            return flight;
        }
        public async Task<bool> AddFLight(FlightM flight)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_config.GetConnectionString("Default"));
                SqlCommand cmd = new SqlCommand("EXEC Admin_AddFlight @from,@to,@seats,@time,@cost", conn);
                cmd.Parameters.AddWithValue("@from", flight.From);
                cmd.Parameters.AddWithValue("@to", flight.To);
                cmd.Parameters.AddWithValue("@seats", flight.seats);
                cmd.Parameters.AddWithValue("@time", flight.flightTime);
                cmd.Parameters.AddWithValue("@cost", flight.cost);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync() == 0 ? false : true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateFlight(FlightM flight)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_config.GetConnectionString("Default"));
                SqlCommand cmd = new SqlCommand("EXEC Admin_UpdateFlight @flightid,@from,@to,@seats,@time,@cost", conn);
                cmd.Parameters.AddWithValue("@flightid", flight.flightId);
                cmd.Parameters.AddWithValue("@from", flight.From);
                cmd.Parameters.AddWithValue("@to", flight.To);
                cmd.Parameters.AddWithValue("@seats", flight.seats);
                cmd.Parameters.AddWithValue("@time", flight.flightTime);
                cmd.Parameters.AddWithValue("@cost", flight.cost);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync() == 0 ? false : true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteFlight(int id)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_config.GetConnectionString("Default"));
                SqlCommand cmd = new SqlCommand("DELETE FROM Flights WHERE FlightId = @flightid", conn);
                cmd.Parameters.AddWithValue("@flightid", id);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync() == 0 ? false : true;
            }
            catch { return false; }
        }
        public async Task<bool> BookFlight(int userid,int flightid)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_config.GetConnectionString("Default"));
                SqlCommand cmd = new SqlCommand("EXEC BookFlight @userid,@flightid", conn);
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@flightid", flightid);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync() == 0 ? false : true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<BookingDetails>> GetBookings()
        {
            List<BookingDetails> bookings = new List<BookingDetails>();
            using SqlConnection conn = new SqlConnection(_config.GetConnectionString("Default"));
            SqlCommand cmd = new SqlCommand("SELECT FlightId,UserId,book_time FROM BookedFlights", conn);
            await conn.OpenAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                BookingDetails booking = new BookingDetails(
                    reader.GetInt32(0), //flightId
                    reader.GetInt32(1), //UserId
                    reader.GetDateTime(2) //Book_time
                );
                bookings.Add(booking);
            }

            reader.Close();
            return bookings;
        }
        public async Task<BookingDetails?> GetBooking(int userid)
        {
            BookingDetails booking = null;
            using SqlConnection conn = new SqlConnection(_config.GetConnectionString("Default"));
            SqlCommand cmd = new SqlCommand("SELECT FlightId,UserId,book_time FROM BookedFlights WHERE userId = @userid", conn);
            cmd.Parameters.AddWithValue("@userid", userid);
            await conn.OpenAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                booking = new BookingDetails(
                    reader.GetInt32(0), //flightId
                    reader.GetInt32(1), //UserId
                    reader.GetDateTime(2) //Book_time
                );
            }

            reader.Close();
            return booking;
        }
    }
}
