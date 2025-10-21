using calorietraker.Context;
using calorietraker.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace calorietraker.Controllers
{
    [ApiController]
    [Route("stats")]
    public class StatsController : ControllerBase
    {
        [Authorize]
        [HttpGet("daily")]
        public async Task<ActionResult<object>> Daily()
        {
            using var db = new AppDbContext();
            var uid = int.Parse(User.Claims.First(c => c.Type == "sub" || c.Type.EndsWith("/nameidentifier")).Value);
            var today = DateTime.UtcNow.Date;
            var meals = await db.Meals.Where(m => m.UserId == uid && m.ConsumedAt >= today && m.ConsumedAt < today.AddDays(1)).ToListAsync();
            var ids = meals.Select(m => m.ProductId).Distinct().ToList();
            var products = await db.Products.Where(p => ids.Contains(p.Id)).ToListAsync();

            decimal totalCalories = 0, protein = 0, fats = 0, carbs = 0;
            foreach (var m in meals)
            {
                var p = products.First(x => x.Id == m.ProductId);
                var k = m.WeightGrams / 100m;
                totalCalories += p.Calories * k;
                protein += (p.Proteins ?? 0) * k;
                fats += (p.Fats ?? 0) * k;
                carbs += (p.Carbs ?? 0) * k;
            }
            return Ok(new { total_calories = totalCalories, protein, fats, carbs });
        }

        [Authorize]
        [HttpGet("weekly")]
        public async Task<ActionResult<IEnumerable<object>>> Weekly()
        {
            using var db = new AppDbContext();
            var uid = int.Parse(User.Claims.First(c => c.Type == "sub" || c.Type.EndsWith("/nameidentifier")).Value);
            var from = DateTime.UtcNow.Date.AddDays(-6);
            var meals = await db.Meals.Where(m => m.UserId == uid && m.ConsumedAt >= from).ToListAsync();
            var ids = meals.Select(m => m.ProductId).Distinct().ToList();
            var products = await db.Products.Where(p => ids.Contains(p.Id)).ToListAsync();

            var days = Enumerable.Range(0, 7).Select(i => from.AddDays(i)).ToList();
            var res = new List<object>();
            foreach (var d in days)
            {
                var dMeals = meals.Where(m => m.ConsumedAt.Date == d.Date).ToList();
                decimal c = 0, pr = 0, f = 0, cr = 0;
                foreach (var m in dMeals)
                {
                    var p = products.First(x => x.Id == m.ProductId);
                    var k = m.WeightGrams / 100m;
                    c += p.Calories * k;
                    pr += (p.Proteins ?? 0) * k;
                    f += (p.Fats ?? 0) * k;
                    cr += (p.Carbs ?? 0) * k;
                }
                res.Add(new { date = d.ToString("yyyy-MM-dd"), total_calories = c, protein = pr, fats = f, carbs = cr });
            }
            return Ok(res);
        }
    }
}
