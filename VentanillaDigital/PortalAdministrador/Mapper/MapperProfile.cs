using ApiGateway.Models;
using AutoMapper;
using PortalAdministrador.Data;
using PortalAdministrador.Data.Account;

namespace PortalAdministrador.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserLogin, LoginFuncionarioModel>()
                .PreserveReferences().ReverseMap();

            CreateMap<AuthenticatedFuncionarioDTO, AuthenticatedUser>()
                .ForMember(target=>target.IsAuthenticated, 
                    options => options.MapFrom(source => source.IsAuthenticated == "true"))
                .PreserveReferences().ReverseMap()
                .ForMember(target => target.IsAuthenticated,
                    options => options.MapFrom(source => source.IsAuthenticated.ToString()));

            CreateMap<AuthenticatedAdministracionDTO, AuthenticatedUser>();

            CreateMap<RegisteredFuncionarioDTO, RegisteredUser>()
                .PreserveReferences().ReverseMap();

            CreateMap<User, PersonaCreateDTO>()
                .PreserveReferences().ReverseMap();

            CreateMap<AuthenticatedUserDTO, AuthenticatedUser>()
                .ForMember(target => target.IsAuthenticated,
                    options => options.MapFrom(source => source.IsAuthenticated == "true"))
                .PreserveReferences().ReverseMap()
                .ForMember(target => target.IsAuthenticated,
                    options => options.MapFrom(source => source.IsAuthenticated.ToString()));

            CreateMap<AccountCreateDTO, UserAccount>()
                .PreserveReferences().ReverseMap();

            CreateMap<AccountUpdateDTO, UpdateUserAccount>()
                .PreserveReferences().ReverseMap();

            CreateMap<UserDeleteRequestDTO, UserDelete>()
                .PreserveReferences().ReverseMap();

            CreateMap<UserLogin,LoginOtpModel>();

            CreateMap<UserDeleteList, PersonaDeleteDTO>()
                .PreserveReferences().ReverseMap();
        }
    }
}
