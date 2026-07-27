using System.Security.Cryptography;
using System.Text;
using MediBook.Application.Dtos;
using MediBook.Application.Interfaces;
using MediBook.Domain.Entities;


namespace MediBook.Application.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenGenerator _tokenGenerator;
        public AuthService(IUserRepository userRepository, ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _tokenGenerator = tokenGenerator;
        }
        public AuthResponseDto? Register(RegisterDto dto)
        {
            if (_userRepository.GetByEmail(dto.Email) != null) return null;
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                Role = "Patient"
            };
            _userRepository.Add(user);
            _userRepository.SaveChanges();
            return new AuthResponseDto { Token = _tokenGenerator.GenerateToken(user), Role = user.Role, Email = user.Email };
        }

        
    public AuthResponseDto? Login(LoginDto dto)
        {
            var user = _userRepository.GetByEmail(dto.Email);
            if (user == null || !VerifyPassword(dto.Password, user.PasswordHash)) return null;
            return new AuthResponseDto { Token = _tokenGenerator.GenerateToken(user), Role = user.Role, Email = user.Email };
        }
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
        private bool VerifyPassword(string password, string hash) => HashPassword(password) == hash;

    }




}
