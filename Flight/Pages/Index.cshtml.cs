using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Flight.Pages
{
    public class IndexModel : PageModel
    {
        public bool login { get; set; } = true;
        public void OnGet()
        {
        }
        public void OnGetLogin(string user)
        {
            login = false;
            ViewData["username"] = user;
        }
    }
}
