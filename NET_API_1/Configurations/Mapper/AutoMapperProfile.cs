using AutoMapper;
using NET_API_1.Models.DTO;
using NET_API_1.Models.Entity;
using NET_API_1.Models.Request;
using NET_API_1.Utils;

namespace NET_API_1.Configurations.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserSignUpModel, User>().ReverseMap();
            CreateMap<PaginatedList<User>, PaginatedList<UserDTO>>().ReverseMap();

            CreateMap<RefreshToken, RefreshTokenDTO>().ReverseMap();
        }
    }
}
