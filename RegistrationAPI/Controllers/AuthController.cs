using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RegistrationAPI.DTOs;
using RegistrationAPI.Models;
using RegistrationAPI.Repositry;

namespace RegistrationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Username and password cannot be empty.");
            }

            var token = await _authRepository.Login(loginDto.Username, loginDto.Password);

            if (token == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(new { Token = token });
        }

        
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Username and password cannot be empty.");
            }

            var user = new AppUser
            { 
                UserName = registerDto.Username, 
                Email = registerDto.Email ,
                PhoneNumber = registerDto.Phone 
                
            };

            var result = await _authRepository.Register(user, registerDto.Password);

            if (!result)
            {
                return BadRequest("User already exists.");
            }

            return Ok("User registered successfully.");
        }
        [HttpGet("Display")]
        public async Task<ActionResult<List<AppUser>>> Display()
        {
            var users = await _authRepository.Display();
            return Ok(users);
        }
    }
}
