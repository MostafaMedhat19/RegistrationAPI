using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RegistrationAPI.Models;

namespace RegistrationAPI.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
       public AppDbContext (DbContextOptions options) : base(options) { }

    }
}
