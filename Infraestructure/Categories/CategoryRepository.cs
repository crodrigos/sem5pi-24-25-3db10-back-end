using dddnet8.Domain.Categories;
using dddnet8.Infraestructure.Shared;

namespace dddnet8.Infraestructure.Categories
{
    public class CategoryRepository : BaseRepository<Category, CategoryId>, ICategoryRepository
    {
    
        public CategoryRepository(ApplicationDbContext context):base(context.Categories)
        {
           
        }


    }
}