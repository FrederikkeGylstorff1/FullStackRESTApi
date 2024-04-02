using RestApiLK.data;

namespace RestApiLK.services
{
    public class kundeService: BaseDBContext<Kunder>
    {
        private readonly LakridsKompanigetDbContext _context;

        public kundeService(LakridsKompanigetDbContext context) : base(context) 
        {
            _context = context;
        }


        //tilføj spesefike metoder her 
    }
}
