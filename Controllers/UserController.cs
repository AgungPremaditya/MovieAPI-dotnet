using Microsoft.AspNetCore.Mvc;
using MovieAPI_dotnet.Dtos.Requests.Users;
using MovieAPI_dotnet.Helpers;
using MovieAPI_dotnet.Models;
using MovieAPI_dotnet.Repositories.Users;

namespace MovieAPI_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher _passwordHasher;

        public UserController(IUserRepository userRepository, PasswordHasher passwordHasher) 
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateRequestDtos createDto)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var user = new User
            {
                Name = createDto.Name,
                Email = createDto.Email,
                Password = _passwordHasher.HashPassword(createDto.Password),
            };

            await _userRepository.AddUser(user);
            return Created();
        }
    }
}
