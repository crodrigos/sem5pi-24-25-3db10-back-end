using dddnet8.Domain.Shared;

namespace dddnet8.Domain.Categories
{
    public interface ICategoryRepository: IRepository<Category, CategoryId>
    {
    }
}