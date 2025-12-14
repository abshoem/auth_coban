using CrudAuthenAuthortruyenthong.Data;
using CrudAuthenAuthortruyenthong.Models.Dto;
using CrudAuthenAuthortruyenthong.Models.Entities;
using CrudAuthenAuthortruyenthong.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CrudAuthenAuthortruyenthong.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IAuthService _auth;

        public UserController(AppDbContext db, IAuthService auth)
        {
            _db = db;
            _auth = auth;
        }
        [Authorize]
        [HttpGet("getProfile")]
        public IActionResult getProfile()
        {
            var pro5 = User.Claims.Select(p => new
            {
                p.Type,
                p.Value
            });

            return Ok(pro5);

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult AdminOnly()
        {
            return Ok("Only admin can access this!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("listuser")]

        public async Task<IActionResult> ListUser()
        {
            var users = await _db.Users.Where(role => role.Role == UserRole.User).Select(u => new UserResponse
            {
                Id = u.Id,
                Username = u.Username,
                Role = u.Role.ToString()
               
            }).ToListAsync();
            
            if (users == null)
            {
                return NotFound("lỗi");
            }
            return Ok(users);
        }

        [Authorize(Roles= "Admin")]
        [HttpPost("deleteuser/{id}")]

        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _db.Users.SingleOrDefaultAsync(uId => uId.Id == id);
            if (user == null)
            {
                throw new Exception("Người dùng không tồn tại");
            }
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Xóa người dùng thành công"
            });
        }

        [Authorize]
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (userName == null)
            {
                return BadRequest();
            }

            await _auth.ChangePassWord(userName, dto.CurrentPassword, dto.NewPassword);

            return Ok(new
            {
                message = "đổi mật khẩu thành công"
            });



        } 

        


    }
}
