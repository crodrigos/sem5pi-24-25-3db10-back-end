using dddnet8.Domain.Families;
using dddnet8.Infraestructure.Shared;

namespace dddnet8.Infraestructure.Families
{
    public class FamilyRepository : BaseRepository<Family, FamilyId>, IFamilyRepository
    {
      
        public FamilyRepository(ApplicationDbContext context):base(context.Families)
        {
            
        }

    }
}