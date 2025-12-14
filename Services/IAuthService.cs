using CrudAuthenAuthortruyenthong.Models.Dto;

namespace CrudAuthenAuthortruyenthong.Services
{
    public interface IAuthService
    {
        Task<TokenResponDto> LoginAsync(string username, string password);
        Task RegisterAsync(string username, string password);
        Task<TokenResponDto> RefreshTokenAsync(string refreshToken);

        Task ChangePassWord(string username, string currentPassword, string newPassword);
    }
}
