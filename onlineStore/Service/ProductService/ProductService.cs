using onlineStore.Data;

namespace onlineStore.Service.ProductService
{
    public class ProductService : IProductService
    {
        private readonly StoreDbContext _context;
        public ProductService(StoreDbContext context) 
        {
            _context = context;

        }
    }
}
