using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAPI_dotnet.Dtos.Requests.Authentication;
using MovieAPI_dotnet.Services;

namespace MovieAPI_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthenticateRequestDto body)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _authService.Authenticate(body);
            if (token == null) return UnprocessableEntity();

            return Ok(token);
        }
    }
}
