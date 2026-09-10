using AutoMapper;
using FDP.Dtos.User;
using FDP.Models;

namespace FDP.Profiles;


public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User,RegisterResponseDto>();
        CreateMap<User,LoginResponseDto>()
                .ForMember(
                    dest=>dest.Token,
                    opt=>opt.Ignore()
                );
    }
}