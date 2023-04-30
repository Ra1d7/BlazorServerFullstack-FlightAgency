using Microsoft.AspNetCore.Mvc.RazorPages;
using static Flight.Pages.loginModel;

namespace Flight.Pages
{
    public class IndexModel : PageModel
    {
        private LoginData _loginData { get; set; } = null;
        public void OnGet()
        {
            //_loginData = RedirectToAction("/login", "LoginData");
        }
    }
}
