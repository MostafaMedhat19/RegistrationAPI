using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using RegistrationAPI.Data;
using RegistrationAPI.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RegistrationAPI.Repositry;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration; 
    public AuthRepository(AppDbContext context , IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string> Login(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        if (user == null)
        {
            return null; // User does not exist
        }

        var passwordHasher = new PasswordHasher<AppUser>();
        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result != PasswordVerificationResult.Success)
        {
            return null;
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecreKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: new[] { new Claim(ClaimTypes.Name, username) },
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);

    }
    public async Task<List<AppUser>> Display()
    {
        return await _context.Users.ToListAsync();
    }
    public async Task<bool> Register(AppUser user, string password)
    {
        if (await _context.Users.AnyAsync(u => u.UserName == user.UserName))
        {
            return false;
        }

     
        var passwordHasher = new PasswordHasher<AppUser>();
        user.PasswordHash = passwordHasher.HashPassword(user, password);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return true;
    }
}
