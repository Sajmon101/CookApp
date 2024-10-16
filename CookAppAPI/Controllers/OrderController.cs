using CookAppAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CookAppAPI.Controllers.DishInOrderController;

namespace CookAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpGet("{tableNum}")]
        public IActionResult GetIdByTable(int tableNum)
        {
            var db = new AppDbContext();
            var order = db.Orders.FirstOrDefault(d => d.TableNum == tableNum && d.IsArch == false);

            if (order == null)
            {
                return NotFound($"Order with name '{tableNum}' not found.");
            }

            return Ok(order);
        }
        [HttpPost]
        public IActionResult Post([FromBody] Order newOrder)
        {
            if (ModelState.IsValid)
            {
                var db = new AppDbContext();
                newOrder.IsArch = false;
                newOrder.Date = DateTime.Now;
                db.Orders.Add(newOrder);
                db.SaveChanges();

                return Ok(newOrder);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPut]
        public IActionResult PlaceOrderInArch(Order order)
        {
            var db = new AppDbContext();
            var update = db.Orders.FirstOrDefault(d => d.TableNum == order.TableNum && d.IsArch == false);
            if(update != null)
            {
                update.IsArch = true;
                db.SaveChanges();
                return Ok();
            }

            return NotFound("Order not found or already archived");
        }
    }
}