using Flight.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Flight.ApiControllers.BookingController;

namespace Flight.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class BookingsModel : PageModel
    {
        [BindProperty]
        public List<BookingDetails> list { get; set; }
        private readonly FlightDB _db;

        public BookingsModel(FlightDB db)
        {
            _db = db;
        }
        public async Task OnGet()
        {
            await Console.Out.WriteLineAsync("test");
        }
        public async Task OnPost()
        {
            RedirectToPage("/bookings");
        }
    }
}
