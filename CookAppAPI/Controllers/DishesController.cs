using CookAppAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CookAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var db = new AppDbContext();
            var list = db.Dishes.OrderBy(d => d.Name).ToList();

            return Ok(list);
        }

        [HttpGet("{dishName}")]
        public IActionResult GetIdByName(string dishName)
        {
            var db = new AppDbContext();
            var dish = db.Dishes.FirstOrDefault(d => d.Name == dishName);

            if (dish == null)
            {
                return NotFound($"Dish with name '{dishName}' not found.");
            }

            return Ok(dish);

        }
    }
}
