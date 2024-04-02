using RestApiLK.data;

namespace RestApiLK.services
{
    public class OrdreService : BaseDBContext<Ordre>
    {
        private readonly LakridsKompanigetDbContext _context;

        public OrdreService(LakridsKompanigetDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
