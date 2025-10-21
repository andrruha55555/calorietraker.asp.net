using calorietraker.Context;
using calorietraker.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace calorietraker.Controllers
{
    [ApiController]
    [Route("achievements")]
    public class AchievementsController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> MyAchievements()
        {
            using var db = new AppDbContext();
            var uid = int.Parse(User.Claims.First(c => c.Type == "sub" || c.Type.EndsWith("/nameidentifier")).Value);
            var list = await db.Achievements.Where(a => a.UserId == uid).ToListAsync();
            return Ok(list);
        }
    }
}
