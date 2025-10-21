using calorietraker.Context;
using calorietraker.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace calorietraker.Controllers
{
    [ApiController]
    [Route("meals")]
    public class MealsController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Meal>>> MyMeals([FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
        {
            using var db = new AppDbContext();
            var uid = int.Parse(User.Claims.First(c => c.Type == "sub" || c.Type.EndsWith("/nameidentifier")).Value);
            var q = db.Meals.Where(m => m.UserId == uid);
            if (from.HasValue) q = q.Where(m => m.ConsumedAt >= from.Value);
            if (to.HasValue) q = q.Where(m => m.ConsumedAt <= to.Value);
            return Ok(await q.OrderByDescending(m => m.ConsumedAt).ToListAsync());
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] MealCreateDto dto)
        {
            using var db = new AppDbContext();
            var uid = int.Parse(User.Claims.First(c => c.Type == "sub" || c.Type.EndsWith("/nameidentifier")).Value);
            var meal = new Meal { UserId = uid, ProductId = dto.product_id, WeightGrams = dto.weight_grams, ConsumedAt = DateTime.UtcNow };
            db.Meals.Add(meal);
            await db.SaveChangesAsync();
            return StatusCode(201);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            using var db = new AppDbContext();
            var uid = int.Parse(User.Claims.First(c => c.Type == "sub" || c.Type.EndsWith("/nameidentifier")).Value);
            var meal = await db.Meals.FirstOrDefaultAsync(m => m.Id == id && m.UserId == uid);
            if (meal == null) return NoContent();
            db.Meals.Remove(meal);
            await db.SaveChangesAsync();
            return NoContent();
        }
    }

    public record MealCreateDto(int product_id, decimal weight_grams);
}
