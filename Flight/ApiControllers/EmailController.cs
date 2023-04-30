using Flight.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Flight.Helpers.EmailSender;

namespace Flight.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EmailController : ControllerBase
    {
        private readonly IEmailSender _emailSender;

        public EmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        [HttpPost]
        public async Task<IActionResult> SendEmailAsync(EmailData data)
        {
            var result = await _emailSender.SendEmailAsync(data);
            return Ok(result);
        }
    }
}
