using APIs;
using APIs.Repository.Interface;
using DAL;

namespace APIs.Repository
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly ApplicationDbContext _context;
        public MaterialRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Material>> GetMaterialsByProductIdAsync(Guid productId)
        {
            return _context.MaterialProducts.Where(mp => mp.ProductId == productId)
                .Select(mp => mp.Material)
                .Distinct()
                .ToList();
        }
    }
}
