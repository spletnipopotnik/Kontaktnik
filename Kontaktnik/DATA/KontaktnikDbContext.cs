using Kontaktnik.Models;
using Microsoft.EntityFrameworkCore;

namespace Kontaktnik.DATA
{
    public class KontaktnikDbContext : DbContext
    {
        public KontaktnikDbContext(DbContextOptions<KontaktnikDbContext> options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ContactType> ContactTypes { get; set; }
        public DbSet<ContactDetail> Contacts { get; set; }
    }
}
