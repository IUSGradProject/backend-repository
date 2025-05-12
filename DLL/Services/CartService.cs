using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIs;
using APIs.Contracts;
using APIs.Repository.Interface;
using AutoMapper;
using BLL.Services.Interface;
using DAL.Repository.Interface;
using DTOs.Contracts;
using Microsoft.CodeAnalysis;

namespace BLL.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IEmailService _emailService;

        public CartService(ICartRepository cartRepository, IMapper mapper, ITokenService tokenService, IUserRepository userRepository, IProductRepository productRepository, IEmailService emailService) 
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _tokenService = tokenService;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _emailService = emailService;
        }

        public async Task CreateCartProduct(CartItemContract cartItem)
        {
            var email = _tokenService.GetEmail();
            var user = await _userRepository.GetUserByEmailAsync(email);
            var cart = await _cartRepository.GetCartByUserId(user.UserId);

            if (cart == null)
            {
                cart = await _cartRepository.CreateCart(new Cart
                {
                    CartId = Guid.NewGuid(),
                    UserId = user.UserId,
                    Paid = 0,
                });
            } 
            var existingItem = await _cartRepository.GetCartByProductAndCart(cartItem.ProductId, cart.CartId);

            if(existingItem != null){
                //Updating Quantity
                existingItem.Quantity = cartItem.Quantity;
                await _cartRepository.UpdateCartProduct(existingItem); // This method should be created 
            }
            else
            {
                //Insert new product
                var cartProduct = _mapper.Map<CartProduct>(cartItem);
                cartProduct.CartId = cart.CartId;
                await _cartRepository.CreateCartProduct(cartProduct, reduceStock: false);
            }
        }

        public async Task CreateCartProducts(CartRequest cartProduct)
        {
            for(int i = 0; i < cartProduct.Cart.Count; i++)
            {
                await CreateCartProduct(cartProduct.Cart[i]);
            }
        }

        public async Task CreateOrder(CartRequest cart)
        {
            var email = _tokenService.GetEmail();
            var user = await _userRepository.GetUserByEmailAsync(email);

            var newCart = await _cartRepository.CreateCart(new Cart
            {
                CartId = Guid.NewGuid(),
                UserId = user.UserId,
                Paid = 1,
                Date = DateTime.UtcNow,
            });

            foreach (var cartItem in cart.Cart)
            {
                await _productRepository.OrderProduct(cartItem.ProductId, cartItem.Quantity);

                var cartProduct = _mapper.Map<CartProduct>(cartItem);
                cartProduct.CartId = newCart.CartId;

                await _cartRepository.CreateCartProduct(cartProduct, reduceStock: false);
                await _cartRepository.DeleteCartItem(email, cartItem.ProductId);
            }

             // Fire-and-forget email sending (non-blocking)
            _ = Task.Run(async () =>
            {
                foreach (var cartItem in cart.Cart)
                {
                    try
                    {
                    await _emailService.SendOrderConfirmationEmailAsync(
                        email, cartItem.Name, cartItem.Price, cartItem.Quantity);
                }
                  catch (Exception ex)
            {
                // Optionally log the exception
                Console.WriteLine($"Email sending failed: {ex.Message}");
            }
             }
         });
        }


        public async Task DeleteCartItem(Guid productId)
        {
            var email = _tokenService.GetEmail();
            await _cartRepository.DeleteCartItem(email, productId);
        }

        public async Task<List<CartItemContract>> GetCartItems()
        {
            var email = _tokenService.GetEmail();
            var user = await _userRepository.GetUserByEmailAsync(email);
            var cartProducts = await _cartRepository.GetCartsByUserId(user.UserId);
            return _mapper.Map<List<CartItemContract>>(cartProducts);
        }

        public async Task<PaginatedDataContract<List<IGrouping<DateTime, OrderItemContract>>>> GetOrders(int pageNumber, int pageSize)
        {
            var email = _tokenService.GetEmail();
            var user = await _userRepository.GetUserByEmailAsync(email);

            var orders = await _cartRepository.GetOrdersByUserId(user.UserId);
            var mappedOrders = _mapper.Map<List<OrderItemContract>>(orders);
            var groupedOrders = mappedOrders.GroupBy(c => c.OrderDate).OrderByDescending(g => g.Key).ToList();

            var responseData = groupedOrders.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var response = new PaginatedDataContract<List<IGrouping<DateTime, OrderItemContract>>> (responseData, pageNumber, pageSize, groupedOrders.Count);

            return response;
        }
    }
}
