using RestApiLK.data;

namespace RestApiLK.services
{
    public class ForhandlerService : BaseDBContext<Forhandler>
    {
        private readonly LakridsKompanigetDbContext _context;

        public ForhandlerService(LakridsKompanigetDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
