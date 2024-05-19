using Microsoft.AspNetCore.Identity;
using MovieAPI_dotnet.Dtos.Requests.Authentication;
using MovieAPI_dotnet.Helpers;
using MovieAPI_dotnet.Models;
using MovieAPI_dotnet.Repositories.Users;

namespace MovieAPI_dotnet.Services
{
    public interface IAuthService
    {
        Task<string?> Authenticate(AuthenticateRequestDto request);
    }
    public class AuthService : IAuthService
    {
        private IUserRepository _userRepository;
        private PasswordHasher _passwordhasher;
        private ITokenService _tokenService;

        public AuthService(IUserRepository UserRepository, PasswordHasher passwordHasher, ITokenService tokenService) 
        {
            _userRepository = UserRepository;
            _passwordhasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<string?> Authenticate(AuthenticateRequestDto request)
        {
            var user = await _userRepository.GetUserByEmail(request.Email);
            if (user == null) return null;

            var verifiedPassword = _passwordhasher.VerifyPassword(request.Password, user.Password);
            if (!verifiedPassword) return null;

            var token = _tokenService.GenerateToken(user);
            if (token == null) return null;

            return token;
        }
    }
}
