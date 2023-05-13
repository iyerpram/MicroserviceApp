using AutoMapper;
using MicroserviceApp.Cart.Application.Dto;
using MicroserviceApp.Cart.Application.RequestHandlers;
using MicroserviceApp.Cart.Domain.Models;
using MicroserviceApp.Common.Abstractions.Database;
using Microsoft.Extensions.Configuration;

namespace MicroserviceApp.Orders.Application
{
    public interface ICartRepository
    {
        Task<CartItemsDto> GetCartItemsAsync(string userId);
        Task<bool> AddItemToCartAsync(AddItemToCart request);
    }

    public class CartRepository : ICartRepository
    {
        private IExtendedDbProvider<Cart.Domain.Models.Cart> _dbProvider { get; }
        public IConfiguration _configuration { get; }
        public IMapper _mapper { get; }

        public CartRepository(IExtendedDbProvider<Cart.Domain.Models.Cart> dbProvider, IConfiguration configuration, IMapper mapper)
        {
            _dbProvider = dbProvider;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<CartItemsDto> GetCartItemsAsync(string userId)
        {
            var items = await _dbProvider.ExecuteQueryAsync<CartItemsDto>( $"select * from cart where userId={userId}");
            return items.FirstOrDefault();
        }

        public async Task<bool> AddItemToCartAsync(AddItemToCart request)
        {
            var items = await _dbProvider.ExecuteQueryAsync<Cart.Domain.Models.Cart>($"select * from Product where userId={request.User.Id}");
            var cart = items.FirstOrDefault();
            if (cart == null)
                return false;

            var product = _mapper.Map<Product>(request.Product);
            cart.Products.Add(product);
            return await _dbProvider.UpdateItemAsync(cart.Id.ToString(), cart);
        }
    }
}
