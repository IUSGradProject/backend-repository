using System.Linq;
using System.Linq.Expressions;
using APIs;
using APIs.Repository.Interface;
using DAL;
using DTOs.Contracts;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace APIs.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;


        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(FilterRequest? filter)
        {
            IQueryable<Product> query = _dbContext.Products
             .Where(p => p.IsDeleted ==  false && p.Available > 0);

            if (!string.IsNullOrEmpty(filter.Query)){
                var searchQuery = filter?.Query.ToLower();

                if (filter?.QueryCategoryId != null)
                {
                    query = query.Where(p => p.CategoryId == filter.QueryCategoryId);
                } 
                query = query.Where(p => p.Name.ToLower().Contains(searchQuery) ||
                            p.Description.ToLower().Contains(searchQuery));
            }

           

            if (filter.Categories.Any())
                query = query.Where(p => filter.Categories.Contains(p.CategoryId));

            if (filter.Styles.Any())
                query = query.Where(p => filter.Styles.Contains(p.StyleId));

            if (filter.Brands.Any())
                query = query.Where(p => filter.Brands.Contains(p.BrandId));

            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);


            return await query.SortBy(filter.SortBy, filter.SortDesc).ToListAsync();

        }

        public async Task<IEnumerable<Product>> GetSoldOutProductsAsync()
        {
            return await _dbContext.Products
                .Where(p => p.IsDeleted == false && p.Available == 0)  
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _dbContext.Products
              .Where(p => p.IsDeleted == false && p.Available > 0)
              .OrderBy(p => p.Name) 
              .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(Guid productId)
        {
            var product = await _dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Power)
                .Include(p => p.Style)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
            return product;
        }

        public async Task AddProductAsync(Product product)
        {
            if (await _dbContext.Products.AnyAsync(p => p.ProductId == product.ProductId))
                throw new InvalidOperationException("Product already exists");
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveCart(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task OrderProduct(Guid productId, int quantity)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
                throw new KeyNotFoundException("Product not found");

            if (product.Available < quantity)
                throw new InvalidOperationException("Not enough stock available");

            product.SoldItems += quantity; 
            product.Available -= quantity; 

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }


        public async Task DeleteProductAsync(Guid productId)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            
            if(product == null)
                throw new KeyNotFoundException("Product not found");

            product.IsDeleted = true;
            _dbContext.Update(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllDeletedProductsAsync()
        {
            return await _dbContext.Products
                                   .Where(p => p.IsDeleted == true)  
                                   .ToListAsync();
        }

        public async Task RestoreProductAsync(Guid productId)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
                throw new KeyNotFoundException("Product not found");

            product.IsDeleted = false;
            _dbContext.Update(product);
            await _dbContext.SaveChangesAsync();
        }


        public async Task UpdateProductAsync(Product product)
        {
            var oldProduct = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == product.ProductId);

            if(oldProduct == null)
                throw new KeyNotFoundException("Product not found");

            oldProduct.IsLast = false;
            product.Version = oldProduct.Version + 1;

            _dbContext.Products.Update(oldProduct);

            product.ProductId = Guid.NewGuid();
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Product> GetProductForEdit(Guid productId)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

            if(product == null)
                throw new KeyNotFoundException("Product not found");

            product.IsEditing = true;
            _dbContext.Update(product);
            await _dbContext.SaveChangesAsync();

            return product;
        }
    }

}
