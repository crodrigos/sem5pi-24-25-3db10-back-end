using dddnet8.Domain.Shared;

namespace dddnet8.Domain.Products
{
    public interface IProductRepository: IRepository<Product,ProductId>
    {
    }
}