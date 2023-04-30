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
        public string username = "not logged in";
        private readonly FlightDB _db;

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
                return RedirectToPage("/index", "Login", new { user = username });
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
