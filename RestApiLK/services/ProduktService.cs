using RestApiLK.data;

namespace RestApiLK.services
{
    public class ProduktService: BaseDBContext<produkter>
    {

        private readonly LakridsKompanigetDbContext _context;

        public ProduktService(LakridsKompanigetDbContext context) : base(context) 
        {
            _context = context;
        }

    }
}
