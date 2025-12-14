using BCrypt.Net;
using CrudAuthenAuthortruyenthong.Data;
using CrudAuthenAuthortruyenthong.Models.Dto;
using CrudAuthenAuthortruyenthong.Models.Entities;
using CrudAuthenAuthortruyenthong.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrudAuthenAuthortruyenthong.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly AppDbContext _db;

        public AuthController(IAuthService auth, AppDbContext db)
        {
            _auth = auth;
            _db = db;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            await _auth.RegisterAsync(dto.Username, dto.Password);

            return Ok("Đăng ký thành công");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _auth.LoginAsync(dto.Username, dto.Password);
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenDto dto)
        {
            var result = await _auth.RefreshTokenAsync(dto.RefreshToken);
            return Ok(result);
        }


       


    }
}
