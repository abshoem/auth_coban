using CrudAuthenAuthortruyenthong.Data;
using CrudAuthenAuthortruyenthong.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudAuthenAuthortruyenthong.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UserController(AppDbContext db)
        {
            _db = db;
        }
        [Authorize]
        [HttpGet("me")]
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
            var users = await _db.Users.Where(role => role.Role == UserRole.User).ToListAsync();
            if (users == null)
            {
                return NotFound("lỗi");
            }
            return Ok(users);
        }

    }
}
