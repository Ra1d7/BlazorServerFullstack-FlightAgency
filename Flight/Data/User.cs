namespace Flight.Data
{
    public class User
    {
        public User(int userId, string username, Roles role)
        {
            this.userId = userId;
            Username = username;
            Role = role;
        }

        public User(string username, string password, string email)
        {
            Username = username;
            Password = password;
            Email = email;
        }

        public User(int userId, string username, string password, string email)
        {
            this.userId = userId;
            Username = username;
            Password = password;
            Email = email;
        }
        public User(int userId, string username, string password, string email,Roles role)
        {
            this.userId = userId;
            Username = username;
            Password = password;
            Email = email;
            Role = role;
        }

        public int userId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<FlightM> BookedFlights { get; set; } = new List<FlightM>();
        public Roles Role { get; set; } = Roles.Client;

    }
}
