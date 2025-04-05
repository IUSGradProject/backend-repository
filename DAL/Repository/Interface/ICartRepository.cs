using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIs;

namespace DAL.Repository.Interface
{
    public interface ICartRepository
    {
        Task CreateCartProduct(CartProduct cartProduct, bool reduceStock = true);


        Task DeleteCartItem(string userEmail, Guid productId);

        Task<Cart> CreateCart(Cart cart);

        Task<Cart> GetCartByUserId(Guid userId);

        Task<List<CartProduct>> GetCartsByUserId(Guid userId);

        Task<List< CartProduct>> GetOrdersByUserId(Guid userId);

        Task<CartProduct> GetCartByProductAndCart(Guid productId, Guid cartId);

    }
}
