using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Ambev.DeveloperEvaluation.Application.DTOs.Auth.Response;
using Ambev.DeveloperEvaluation.Application.Commands.Auth;
using Ambev.DeveloperEvaluation.Application.Commands.Users;
using Ambev.DeveloperEvaluation.Application.Commands.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Commands.Carts;
using Ambev.DeveloperEvaluation.Application.Commands.Products;
using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Application.DTOs.Carts.Response;
using Ambev.DeveloperEvaluation.Application.DTOs.Products.Response;
using Ambev.DeveloperEvaluation.Application.DTOs.Sales.Response;
using Ambev.DeveloperEvaluation.Application.DTOs.Users.Response;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.ListCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;
using Ambev.DeveloperEvaluation.Application.DTOs.Sales;
using Ambev.DeveloperEvaluation.Application.DTOs.Carts;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

/// <summary>
/// Mapping profile for AutoMapper.
/// This profile maps the Application layer DTOs to the WebApi Response models and converts API Requests to Application Commands.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<User, AuthenticateUserResponse>()
            .ForMember(dest => dest.Token, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
        CreateMap<AuthenticateUserRequest, AuthenticateUserCommand>();
        CreateMap<AuthenticateUserResponseDto, AuthenticateUserResponse>();

        CreateMap<CreateUserCommand, User>();
        CreateMap<CreateUserRequest, CreateUserCommand>();
        CreateMap<User, CreateUserResponseDto>();
        CreateMap<CreateUserResponseDto, CreateUserResponse>();

        CreateMap<Guid, DeleteUserCommand>()
            .ConstructUsing(id => new DeleteUserCommand(id));
        CreateMap<Guid, GetUserCommand>()
            .ConstructUsing(id => new GetUserCommand(id));
        CreateMap<User, GetUserResponseDto>();
        CreateMap<GetUserResponseDto, GetUserResponse>();
        CreateMap<GetUserResponseDto, ListUsersResponse>();
        CreateMap<User, ListUsersResponse>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        CreateMap<ListUsersRequest, ListUsersCommand>();
        CreateMap<UpdateUserRequest, UpdateUserCommand>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<User, UpdateUserResponseDto>();
        CreateMap<UpdateUserResponseDto, UpdateUserResponse>();

        CreateMap<CreateSaleCommand, Sale>();
        CreateMap<CreateSaleRequest, CreateSaleCommand>();
        CreateMap<CreateSaleItemRequest, CreateSaleItemDto>();
        CreateMap<CreateSaleResponse, CreateSaleResponseDto>();
        CreateMap<CreateSaleResponseDto, CreateSaleResponse>();

        CreateMap<Sale, GetSaleResponseDto>();
        CreateMap<SaleItem, GetSaleItemDto>();
        CreateMap<GetSaleRequest, GetSaleCommand>()
            .ConstructUsing(req => new GetSaleCommand(req.Id));
        CreateMap<GetSaleResponse, GetSaleResponseDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        CreateMap<GetSaleResponseDto, GetSaleResponse>();
        CreateMap<GetSaleItemDto, GetSaleItemResponse>();

        CreateMap<GetSaleResponseDto, ListSalesResponse>();
        CreateMap<ListSalesRequest, ListSalesCommand>();
        CreateMap<GetSaleResponse, ListSalesResponseDto>();
        CreateMap<ListSalesResponseDto, ListSalesResponse>();

        CreateMap<UpdateSaleRequest, UpdateSaleCommand>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UpdateSaleResponse, UpdateSaleResponseDto>();
        CreateMap<UpdateSaleResponseDto, UpdateSaleResponse>();

        CreateMap<DeleteSaleRequest, DeleteSaleCommand>()
            .ConstructUsing(req => new DeleteSaleCommand(req.Id));

        CreateMap<CreateProductCommand, Product>();
        CreateMap<CreateProductRequest, CreateProductCommand>();
        CreateMap<CreateProductResponse, CreateProductResponseDto>();
        CreateMap<CreateProductResponseDto, CreateProductResponse>();

        CreateMap<Product, GetProductResponseDto>();
        CreateMap<GetProductRequest, GetProductCommand>()
            .ConstructUsing(req => new GetProductCommand(req.Id));
        CreateMap<GetProductResponse, GetProductResponseDto>();
        CreateMap<GetProductResponseDto, GetProductResponse>();

        CreateMap<GetProductResponseDto, ListProductsResponse>();
        CreateMap<ListProductsRequest, ListProductsCommand>();
        CreateMap<GetProductResponse, ListProductsResponseDto>();
        CreateMap<ListProductsResponseDto, ListProductsResponse>();

        CreateMap<UpdateProductRequest, UpdateProductCommand>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UpdateProductResponse, UpdateProductResponseDto>();
        CreateMap<UpdateProductResponseDto, UpdateProductResponse>();

        CreateMap<DeleteProductRequest, DeleteProductCommand>()
            .ConstructUsing(req => new DeleteProductCommand(req.Id));

        CreateMap<CreateCartCommand, Cart>();
        CreateMap<CreateCartRequest, CreateCartCommand>();
        CreateMap<CreateCartItemRequest, CreateCartItemDto>();
        CreateMap<CreateCartResponse, CreateCartResponseDto>();
        CreateMap<CreateCartResponseDto, CreateCartResponse>();

        CreateMap<Cart, GetCartResponseDto>();
        CreateMap<CartItem, GetCartItemDto>();
        CreateMap<GetCartRequest, GetCartCommand>()
            .ConstructUsing(req => new GetCartCommand(req.Id));
        CreateMap<GetCartResponse, GetCartResponseDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        CreateMap<GetCartResponseDto, GetCartResponse>();
        CreateMap<GetCartItemDto, GetCartItemResponse>();
        CreateMap<GetCartResponseDto, ListCartsResponse>();

        CreateMap<ListCartsRequest, ListCartsCommand>();
        CreateMap<GetCartResponse, ListCartsResponseDto>();
        CreateMap<ListCartsResponseDto, ListCartsResponse>();

        CreateMap<UpdateCartRequest, UpdateCartCommand>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UpdateCartItemRequest, UpdateCartItemDto>();
        CreateMap<UpdateCartResponse, UpdateCartResponseDto>();
        CreateMap<UpdateCartResponseDto, UpdateCartResponse>();

        CreateMap<DeleteCartRequest, DeleteCartCommand>()
            .ConstructUsing(req => new DeleteCartCommand(req.Id));
    }
}
