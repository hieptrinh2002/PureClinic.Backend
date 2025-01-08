using AutoMapper;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.Business.Schedule;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.API.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // RefreshToken mappings
            CreateMap<RefreshToken, RefreshTokenViewModel>().ReverseMap();
            CreateMap<RefreshTokenCreateViewModel, RefreshToken>().ReverseMap();

            // Product mappings
            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<ProductCreateViewModel, Product>().ReverseMap();
            CreateMap<ProductUpdateViewModel, Product>().ReverseMap();

            // Role mappings
            CreateMap<Role, RoleViewModel>().ReverseMap();
            CreateMap<RoleCreateViewModel, Role>().ReverseMap();
            CreateMap<RoleUpdateViewModel, Role>().ReverseMap();

            // User mappings
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<User, PatientViewModel>().ReverseMap();

            //Schedule
            CreateMap<WorkWeek, WorkWeekScheduleViewModel>().ReverseMap();

            //// Paginated data mappings (if needed)
            //CreateMap<PaginatedDataViewModel<Product>, PaginatedDataViewModel<ProductViewModel>>();
            //CreateMap<PaginatedDataViewModel<Role>, PaginatedDataViewModel<RoleViewModel>>();
            //CreateMap<PaginatedDataViewModel<User>, PaginatedDataViewModel<UserViewModel>>();
            //CreateMap<PaginatedDataViewModel<RefreshToken>, PaginatedDataViewModel<RefreshTokenViewModel>>();
        }
    }
}
