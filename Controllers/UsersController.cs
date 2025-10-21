using calorietraker.Context;
using calorietraker.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace calorietraker.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<object>> Me()
        {
            using var db = new AppDbContext();
            var id = int.Parse(User.Claims.First(c => c.Type == "sub" || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            var me = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (me == null) return NotFound();
            return Ok(me);
        }
    }
}
