using RestApiLK.data;

namespace RestApiLK.services
{
    public class BetalingService : BaseDBContext<Betaling>
    {
        private readonly LakridsKompanigetDbContext _context;

        public BetalingService(LakridsKompanigetDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
