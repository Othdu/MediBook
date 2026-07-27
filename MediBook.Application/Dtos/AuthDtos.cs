using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediBook.Application.Dtos
{
    public class RegisterDto
    {
        [Required,EmailAddress]
        public string Email { get; set; }= string.Empty;
        [Required,MinLength(6)]
        public string Password { get; set; }= string.Empty;
    }
    public class LoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

    }
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; }= string.Empty;
    }
}
