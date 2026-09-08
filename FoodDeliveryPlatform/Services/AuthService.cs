using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using FDP.Data;
using FDP.Dtos.User;
using FDP.Interface;
using FDP.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FDP.Services.Auth;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context,IConfiguration configuration)
    {
        _context=context;
        _configuration=configuration;
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

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return new RegisterResponseDto
        {
            FirstName=user.FirstName,
            LastName=user.LastName,
            Email=user.Email,
            Address=user.Address,
            Role=user.Role
        };

    }   

    public async Task<LoginResponseDto?> LoginUserAsnyc(LoginUserDto dto)
    {
     var user=await _context.Users 
        .FirstOrDefaultAsync(u=>u.Email==dto.Email);

        if (user == null)
        {
            return null;
        }

        if (!BCrypt.Net.BCrypt.Verify(dto.Password,user.Password))
        {
            return null;
        }

        var token=GenerateToken(user);

        return new LoginResponseDto
        {
            Id=user.Id,
            FirstName=user.FirstName,
            LastName=user.LastName,
            Email=user.Email,
            Token=token,
            Role=user.Role,
        };
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