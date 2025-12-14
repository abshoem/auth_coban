using System.Linq.Expressions;

namespace CrudAuthenAuthortruyenthong.Models.Entities
{
    public class User
    {
        public int Id {  get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }

   
}
