using Flight.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using static Flight.Helpers.EmailSender;

namespace Flight.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ContactModel : PageModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ContactModel> _logger;
        private readonly IConfiguration _config;

        public async void OnPostAsync()
        {
            var fullname = Request.Form["fullname"];
            var email = Request.Form["email"];
            var phone = Request.Form["phone"];
            var message = Request.Form["message"];

            var data = new EmailData(fullname,email,phone,message);

                var _emailSender = new EmailSender(_config);
            await _emailSender.SendEmailAsync(data);
            ViewData["confirm"] = $"thank you {fullname}, we will forward your message";

        }

        public ContactModel(ILogger<ContactModel> logger,IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}