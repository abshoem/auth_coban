using CrudAuthenAuthortruyenthong.Data;
using CrudAuthenAuthortruyenthong.Models.Dto;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using CrudAuthenAuthortruyenthong.Models.Entities;
using Microsoft.EntityFrameworkCore;
namespace CrudAuthenAuthortruyenthong.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public AuthService(AppDbContext context, TokenService tokenService) {
            _context = context;
            _tokenService = tokenService;
        }
        
        public async Task<TokenResponDto> LoginAsync(string username, string password)
        {
            var user = _context.Users.Include(rf => rf.RefreshTokens).SingleOrDefault(un => un.Username == username);
            if(user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash ))
            {
                throw new Exception("Tài khoản hoặc mật khẩu không chính xác");
            }

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return new TokenResponDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };


        }

        public async Task<TokenResponDto> RefreshTokenAsync(string refreshToken)
        {
            var tokenEntity = _context.RefreshTokens.Include(r => r.User).SingleOrDefault(r => r.Token == refreshToken);

            if (tokenEntity == null || tokenEntity.IsExpired)
            {
                throw new Exception("Refresh token lỗi hoặc hết hạn");
            }

            var user = tokenEntity.User;

            _context.RefreshTokens.Remove(tokenEntity);

            var accessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            return new TokenResponDto { 
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.Token
            };
        }

        public async Task RegisterAsync(string username, string password)
        {
            var user = new User
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = UserRole.User
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

        }

        public async Task ChangePassWord(string username, string currentPassword, string newPassword)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            
            if (currentPassword == newPassword)
            {
                throw new Exception("Mật khẩu mới không được trùng với mật khẩu hiện tại");
            }

            if (user == null )
            {
                throw new Exception("không thấy người dùng");
            }

            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
            {
                throw new Exception("Mật khẩu hiện tại không đúng");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            await _context.SaveChangesAsync();

        }




    }
}
