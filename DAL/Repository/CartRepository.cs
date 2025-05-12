using APIs;
using DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> CreateCart(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }


        public async Task UpdateCartProduct(CartProduct product){
            _context.CartProduct.Update(product);
            await _context.SaveChangesAsync();
        }



        public async Task CreateCartProduct(CartProduct cartProduct, bool reduceStock = true)
        {
            var item = await _context.CartProducts
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.CartId == cartProduct.CartId && c.ProductId == cartProduct.ProductId);

            if (item != null)
            {
                if (!reduceStock || item.Product.Available >= cartProduct.Quantity)
                {
                    item.Quantity += cartProduct.Quantity;
                    if (reduceStock)
                        item.Product.Available -= cartProduct.Quantity;

                    _context.Update(item);
                }
                else
                {
                    throw new InvalidOperationException("Not enough stock available");
                }
            }
            else
            {
                var product = await _context.Products.FindAsync(cartProduct.ProductId);
                if (product != null && (!reduceStock || product.Available >= cartProduct.Quantity))
                {
                    if (reduceStock)
                        product.Available -= cartProduct.Quantity;

                    _context.Products.Update(product);
                    _context.CartProducts.Add(cartProduct);
                }
                else
                {
                    throw new InvalidOperationException("Not enough stock available");
                }
            }

            await _context.SaveChangesAsync();
        }



        public async Task DeleteCartItem(string userEmail, Guid productId)
        {
            var cartProduct = await _context.CartProducts.FirstOrDefaultAsync(cp => cp.ProductId == productId && cp.Cart.User.Email == userEmail && cp.Cart.Paid == 0);
            if (cartProduct != null)
            {
                _context.CartProducts.Remove(cartProduct);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<CartProduct> GetCartByProductAndCart(Guid productId, Guid cartId)
        {
            return await _context.CartProducts.FirstOrDefaultAsync(c => c.ProductId == productId && c.CartId == cartId);
        }

        public async Task<Cart> GetCartByUserId(Guid userId)
        {
            return await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId && c.Paid == 0);
        }

        public async Task<List<CartProduct>> GetCartsByUserId(Guid userId)
        {
            return await _context.CartProducts.Include(c => c.Product).Where(c => c.Cart.UserId == userId && c.Cart.Paid == 0).ToListAsync();
        }

        public async Task<List<CartProduct>> GetOrdersByUserId(Guid userId)
        {
            return await _context.CartProducts
                .Include(c => c.Product).IgnoreQueryFilters()
                .Include(c => c.Cart)
                .Where(c => c.Cart.UserId == userId && c.Cart.Paid == 1)
                .ToListAsync();
        }
    }
}
