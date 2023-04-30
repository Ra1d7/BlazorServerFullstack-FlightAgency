using Flight.Data;
using Flight.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Net.Mime.MediaTypeNames;

namespace Flight.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class loginModel : PageModel
    {
        public bool login_status { get; set; }
        public Roles Role { get; set; } = Roles.None;
        public string loggedin_username { get; set; }
        private readonly FlightDB _db;
        public record LoginData(bool login_status,string loggedin_username, Roles Role = Roles.None);
        public loginModel(FlightDB db)
        {
            _db = db;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            var username = Request.Form["username"];
            var password = Request.Form["password"];

            bool result = await _db.LoginUser(username, password);

            if (result)
            {
                ViewData["color"] = "text-success";
                ViewData["status"] = $"Welcome, {username}!";
                login_status = true;
                loggedin_username = username;
                Role = await _db.GetRole(username);
                return RedirectToPage("/index");
            }
            else
            {
                ViewData["color"] = "text-danger";
                ViewData["status"] = $"Wrong username or password";
                return Page();
            }
        }
    }
}
