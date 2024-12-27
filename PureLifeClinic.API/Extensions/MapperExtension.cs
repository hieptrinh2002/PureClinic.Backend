using AutoMapper;
using Project.Core.Entities.Business;
using Project.Core.Entities.General;
using Project.Core.Interfaces.IMapper;
using Project.Core.Mapper;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;

namespace Project.API.Extensions
{
    public static class MapperExtension
    {
        public static IServiceCollection RegisterMapperService(this IServiceCollection services)
        {

            #region Mapper

            services.AddSingleton<IMapper>(sp => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RefreshToken, RefreshTokenViewModel>().ReverseMap();
                cfg.CreateMap<RefreshTokenCreateViewModel, RefreshToken>();

                cfg.CreateMap<Product, ProductViewModel>().ReverseMap();
                cfg.CreateMap<ProductCreateViewModel, Product>().ReverseMap();

                cfg.CreateMap<Role, RoleViewModel>().ReverseMap();
                cfg.CreateMap<RoleCreateViewModel, Role>();
                cfg.CreateMap<RoleUpdateViewModel, Role>();

                cfg.CreateMap<User, UserViewModel>().ReverseMap();

            }).CreateMapper());

            // Register the IMapperService implementation with your dependency injection container
            services.AddSingleton<IBaseMapper<Product, ProductViewModel>, BaseMapper<Product, ProductViewModel>>();
            services.AddSingleton<IBaseMapper<ProductCreateViewModel, Product>, BaseMapper<ProductCreateViewModel, Product>>();
            services.AddSingleton<IBaseMapper<ProductUpdateViewModel, Product>, BaseMapper<ProductUpdateViewModel, Product>>();

            services.AddSingleton<IBaseMapper<Role, RoleViewModel>, BaseMapper<Role, RoleViewModel>>();
            services.AddSingleton<IBaseMapper<RoleCreateViewModel, Role>, BaseMapper<RoleCreateViewModel, Role>>();
            services.AddSingleton<IBaseMapper<RoleUpdateViewModel, Role>, BaseMapper<RoleUpdateViewModel, Role>>();

            services.AddSingleton<IBaseMapper<User, UserViewModel>, BaseMapper<User, UserViewModel>>();
            services.AddSingleton<IBaseMapper<UserViewModel, User>, BaseMapper<UserViewModel, User>>();

            services.AddSingleton<IBaseMapper<RefreshTokenCreateViewModel, RefreshToken>, BaseMapper<RefreshTokenCreateViewModel, RefreshToken>>();
            services.AddSingleton<IBaseMapper<RefreshToken, RefreshTokenViewModel>, BaseMapper<RefreshToken, RefreshTokenViewModel>>();

            #endregion

            return services;
        }
    }
}
