using FDP.Dtos.User;

namespace FDP.Interface;

public interface IAuthService
{
    public Task<RegisterResponseDto> RegisterUserAsync(RegisterUserDto dto);
    public Task<LoginResponseDto?> LoginUserAsnyc(LoginUserDto dto);
}