using calorietraker.Context;
using calorietraker.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace calorietraker.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> List()
        {
            using var db = new AppDbContext();
            return Ok(await db.Products.ToListAsync());
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Product p)
        {
            using var db = new AppDbContext();
            db.Products.Add(p);
            await db.SaveChangesAsync();
            return StatusCode(201);
        }
    }
}
