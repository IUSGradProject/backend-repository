using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIs;
using APIs.Contracts;
using DTOs.Contracts;

namespace BLL.Services.Interface
{
    public interface ICartService
    {
        Task CreateCartProduct(CartItemContract cartProduct);

        Task CreateCartProducts(CartRequest cart);

        Task DeleteCartItem(Guid cartProductId);

        Task<List<CartItemContract>> GetCartItems();

        Task CreateOrder(CartRequest cart);

        Task<PaginatedDataContract<List<IGrouping<DateTime, OrderItemContract>>>> GetOrders(int pageNumber, int pageSize);
    }
}
