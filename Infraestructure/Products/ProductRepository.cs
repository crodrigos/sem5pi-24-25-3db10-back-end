using dddnet8.Domain.Products;
using dddnet8.Infraestructure.Shared;

namespace dddnet8.Infraestructure.Products
{
    public class ProductRepository : BaseRepository<Product, ProductId>,IProductRepository
    {
        public ProductRepository(ApplicationDbContext context):base(context.Products)
        {
           
        }
    }
}