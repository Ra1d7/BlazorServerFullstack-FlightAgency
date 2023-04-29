using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Flight.Data
{
	public class IdentityContext : IdentityDbContext
	{
        public IdentityContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
