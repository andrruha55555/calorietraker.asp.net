using calorietraker.Context;
using calorietraker.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace calorietraker.Controllers
{
    [ApiController]
    [Route("goals")]
    public class GoalsController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Goal>>> List()
        {
            using var db = new AppDbContext();
            var uid = int.Parse(User.Claims.First(c => c.Type == "sub" || c.Type.EndsWith("/nameidentifier")).Value);
            return Ok(await db.Goals.Where(g => g.UserId == uid).ToListAsync());
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] GoalCreateDto dto)
        {
            using var db = new AppDbContext();
            var uid = int.Parse(User.Claims.First(c => c.Type == "sub" || c.Type.EndsWith("/nameidentifier")).Value);
            var g = new Goal { UserId = uid, TargetType = dto.target_type, TargetValue = dto.target_value, StartDate = dto.start_date, EndDate = dto.end_date, IsCompleted = false };
            db.Goals.Add(g);
            await db.SaveChangesAsync();
            return StatusCode(201);
        }
    }

    public record GoalCreateDto(string target_type, decimal? target_value, DateTime? start_date, DateTime? end_date);
}
