using MediBook.Application.Services;
using MediBook.Application.Dtos;
using Microsoft.AspNetCore.Mvc;



namespace MediBook.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("Register")]
        public ActionResult Register(RegisterDto dto)
        {
            var result = _authService.Register(dto);
            if (result == null)

                return BadRequest("Email already exists");
            return Ok(result);


        }
        [HttpPost("Login")]
        public ActionResult Login(LoginDto dto)
        {
            var result = _authService.Login(dto);
            if (result == null) return Unauthorized("invalid email or password.");
            return Ok(result);
        }
    }
}
