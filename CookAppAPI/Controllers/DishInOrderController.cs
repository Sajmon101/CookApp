using CookAppAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;

namespace CookAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishInOrderController : ControllerBase
    {
        public class DishOrderInfo
        {
            public int TableNum { get; set; }
            public string Date { get; set; }
            public string Name { get; set; }
            public int DishInOrderId { get; set; }
        }

        public class OrderInfo
        {
            public int Price { get; set; }
            public string Date { get; set; }
            public string Name { get; set; }
            public int DishInOrderId { get; set; }
        }

        public class IdInfo
        {
            public int dishId { get; set; }
            public int orderId { get; set; }
            public int employeeId { get; set; }
            public int dishInOrderId { get; set; }
        }

        public class CookerDish
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        [HttpGet("forCooker/{employeeId}")]
        public IActionResult GetDishesForCooker(int employeeId)
        {
            var db = new AppDbContext();

            var result = db.DishInOrders
                                .Include(dio => dio.Dish)
                                .Include(dio => dio.Order)
                                .Include(dio => dio.Executives)
                                .Where(dio => !dio.Order.IsArch && !dio.KitchenCheck
                                 && dio.Executives.Any(e => e.EmployeeId == employeeId))
                                .OrderBy(dio => dio.Id)
                                .Select(dio => new CookerDish
                                {
                                    Id = dio.Id,
                                    Name = dio.Dish.Name
                                }).ToList();

            return Ok(result);
        }


        [HttpGet("byId/{employeeId}")]
        public IActionResult GetDishesByEmployee(int employeeId)
        {
            var db = new AppDbContext();

            var query = db.DishInOrders
                                .Include(dio => dio.Dish)
                                .Include(dio => dio.Order)
                                .Include(dio => dio.Executives)
                                .Where(dio => !dio.Order.IsArch && !dio.WaiterCheck && dio.KitchenCheck
                                 && dio.Executives.Any(e => e.EmployeeId == employeeId))
                                .Select(dio => new DishOrderInfo
                                {
                                    DishInOrderId = dio.Id,
                                    TableNum = dio.Order.TableNum,
                                    Name = dio.Dish.Name,
                                    Date = dio.Order.Date.ToString("HH:mm:ss")
                                });

            var result = query.ToList();

            if (result.Count > 0)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpGet("byTable/{tableNum}")]
        public IActionResult GetDishesInfoByTable(int tableNum)
        {
            var db = new AppDbContext();

            var query = db.DishInOrders
                                .Include(dio => dio.Dish)
                                .Include(dio => dio.Order)
                                .Where(dio => dio.Order.TableNum == tableNum && !dio.Order.IsArch)
                                .Select(dio => new OrderInfo
                                {
                                    DishInOrderId = dio.Id,
                                    Price = (int)dio.Dish.Price,
                                    Name = dio.Dish.Name,
                                    Date = dio.Order.Date.ToString("HH:mm:ss")
                                });

            var result = query.ToList();

            return Ok(result);
        }        
        
        [HttpGet("{dishInOrderId}")]
        public IActionResult GetDishesInfoById(int dishInOrderId)
        {
            var db = new AppDbContext();

            DishInOrder found = db.DishInOrders.Find(dishInOrderId);

            return Ok(found);
        }

        [HttpPut]
        public IActionResult UpdateWaiterCheck(DishOrderInfo dio)
        {
            if (ModelState.IsValid)
            {
                var db = new AppDbContext();
                DishInOrder update = db.DishInOrders.Find(dio.DishInOrderId);

                update.WaiterCheck = true;
                db.SaveChanges();

                return NoContent();
            }
            else return BadRequest();
        }
        [HttpPost]
        public IActionResult Post([FromBody] IdInfo idInfo)
        {
            DishInOrder newDishInOrder = new();

            if (ModelState.IsValid)
            {
                var db = new AppDbContext();
                var dish = db.Dishes.Find(idInfo.dishId);
                var order = db.Orders.Find(idInfo.orderId);
                if (dish != null && order != null)
                {
                    newDishInOrder = new DishInOrder
                    {
                        DishId = dish.Id,
                        OrderId = order.Id,
                        Dish = dish,
                        Order = order
                    };
                    db.DishInOrders.Add(newDishInOrder);
                    db.SaveChanges();
                    return Ok(newDishInOrder.Id);
                }

                return NotFound();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
