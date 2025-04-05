using APIs;
using APIs.Repository.Interface;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace APIs.Repository
{
    public class AttributeRepository : IAttributeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AttributeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
        {
            return await _dbContext.Brands.ToListAsync();
        }

        public async Task<IEnumerable<Style>> GetAllStylesAsync()
        {
            return await _dbContext.Styles.ToListAsync();
        }

        public async Task<IEnumerable<Power>> GetAllPowersAsync()
        {
            return await _dbContext.Powers.ToListAsync();
        }
    }
}
