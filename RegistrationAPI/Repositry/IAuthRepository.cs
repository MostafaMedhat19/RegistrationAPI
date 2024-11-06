using RegistrationAPI.Models;

namespace RegistrationAPI.Repositry
{
    public interface IAuthRepository
    {
        Task<string> Login(string username, string password);
        Task<bool> Register(AppUser user, string password);
        Task<List<AppUser>> Display();
    }
}
