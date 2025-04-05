using AutoMapper;
using APIs.Contracts;
using APIs.Contracts.Product;
using DTOs.Contracts;
using APIs;
using DTOs.Contracts.Product;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Build.Framework;

namespace BLL.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Material, LoV>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MaterialId));
            CreateMap<Category, LoV>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId));
            CreateMap<Style, LoV>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StyleId));
            CreateMap<Brand, LoV>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BrandId));
            CreateMap<Power, LoV>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PowerId));
            CreateMap<Product, ProductContract>();
            CreateMap<Product, ProductBaseContract>();
            CreateMap<UpdateProductContract, Product>()
                .ForMember(dest => dest.IsLast, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ReverseMap();
            CreateMap<Product, ProductOverviewContract>();
            CreateMap<CartProduct, CartItemContract>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Product.Image))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.Available, opt => opt.MapFrom(src => src.Product.Available));
            CreateMap<CartProduct, OrderItemContract>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Product.Image))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.Available, opt => opt.MapFrom(src => src.Product.Available))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.Cart.Date));
            CreateMap<CartItemContract, CartProduct>()
                .ForMember(dest => dest.CartProductId, opt => opt.MapFrom(src => Guid.NewGuid()));
            CreateMap<CreateUserRequest, User>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => 1));
            CreateMap<Role, RoleResponse>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role1));
        }
    }
}
