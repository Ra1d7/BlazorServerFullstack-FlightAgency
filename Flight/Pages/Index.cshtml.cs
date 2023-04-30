using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Flight.Pages
{
    public class IndexModel : PageModel
    {
        public string login { get; set; } = "Login";
        public void OnGet()
        {
            ViewData["login"] = login;
        }
        public void OnGetLogin(string user)
        {
            ViewData["login"] = "";
            ViewData["username"] = user;
        }
    }
}
