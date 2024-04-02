using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;

namespace RestApiLK.data
{
    public class LakridsKompanigetDbContext : DbContext
    {
        public LakridsKompanigetDbContext(DbContextOptions<LakridsKompanigetDbContext> options) : base(options)
        {
        }

        public DbSet<Kunder> Kunder { get; set; }
        public DbSet<produkter> Produkter { get; set; }
        public DbSet<Forhandler> Forhandlere { get; set; }
        public DbSet<Ordre> Ordre { get; set; }
        public DbSet<Betaling> Betalinger { get; set; }
    }
}
