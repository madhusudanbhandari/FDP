using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FDP.Dtos.User;
using FDP.Interface;
using FDP.Models;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;

namespace FDP.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IConfiguration _configuration;

    private readonly IMapper _mapper;

    public AuthService(IAuthRepository authRepository,IConfiguration configuration,IMapper mapper)
    {
        _authRepository=authRepository;
        _configuration=configuration;
        _mapper=mapper;
    }

    public async Task<RegisterResponseDto> RegisterUserAsync(RegisterUserDto dto)
    {
        var user=new User
        {
            FirstName=dto.FirstName,
            LastName=dto.LastName,
            Email=dto.Email,
            Password=BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Address=dto.Address,
            Role=dto.Role
        };

        await _authRepository.AddAsync(user);
        await _authRepository.SaveChangesAsync();

        // return new RegisterResponseDto
        // {
        //     FirstName=user.FirstName,
        //     LastName=user.LastName,
        //     Email=user.Email,
        //     Address=user.Address,
        //     Role=user.Role
        // };

        return _mapper.Map<RegisterResponseDto>(user);

    }   

    public async Task<LoginResponseDto?> LoginUserAsnyc(LoginUserDto dto)
    {
     var user=await _authRepository.GetByEmailAsync(dto.Email);

        if (user == null)
        {
            return null;
        }

        if (!BCrypt.Net.BCrypt.Verify(dto.Password,user.Password))
        {
            return null;
        }

        var token=GenerateToken(user);

        // return new LoginResponseDto
        // {
        //     Id=user.Id,
        //     FirstName=user.FirstName,
        //     LastName=user.LastName,
        //     Email=user.Email,
        //     Token=token,
        //     Role=user.Role,
        // };

        var response= _mapper.Map<LoginResponseDto>(user);
        response.Token=token;
        return response;
    }

    private string GenerateToken(User user)
    {
        var claims=new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key=new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!
            )
        );
        
        var credentials=new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token=new JwtSecurityToken(
            issuer:_configuration["Jwt:Issuer"],
            audience:_configuration["Jwt:Audience"],
            claims:claims,
            expires: DateTime.UtcNow.AddMinutes(
                _configuration.GetValue<double>("Jwt:ExpirationMinutes")
            ),
            signingCredentials:credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
        
    }

}