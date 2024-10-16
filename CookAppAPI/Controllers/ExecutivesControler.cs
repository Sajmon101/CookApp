using CookAppAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CookAppAPI.Controllers.DishInOrderController;

namespace CookAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutivesControler : ControllerBase
    {
        public class IdInfo2
        {
            public int employeeId { get; set; }
            public int dishInOrderId { get; set; }
        }

        public class CookerDish
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        [HttpGet]
        public IActionResult GetDishesInQueue()
        {
            var db = new AppDbContext();

            var dishesWithoutCooker = db.DishInOrders
                                    .Where(dishInOrder => !dishInOrder.Executives.Any(executive =>
                                     executive.Employee.EmployeeType.TypeName == "Kucharz"))
                                    .ToList();

            int queueLength = dishesWithoutCooker.Count();

            return Ok(queueLength);
        }

        [HttpPost]
        public IActionResult Post2([FromBody] IdInfo2 idInfo)
        {
            if (ModelState.IsValid)
            {
                var db = new AppDbContext();
                var employee = db.Employees.Find(idInfo.employeeId);
                var dishInOrder = db.DishInOrders.Find(idInfo.dishInOrderId);

                //if (employee != null && dishInOrder != null)
                //{
                    // Create new Executive entry
                    var newExecutive = new Executive
                    {
                        EmployeeId = idInfo.employeeId,
                        DishInOrderId = dishInOrder.Id, // Set to the ID of the newly created DishInOrder
                        Employee = employee,
                        DishInOrder = dishInOrder
                    };
                    db.Executives.Add(newExecutive);

                    db.SaveChanges(); // Save again to store the new Executive record

                    return Ok();
                //}

                //return NotFound("Dish or Order not found.");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("forCooker")]
        public IActionResult AssignDishToCooker(IdInfo2 employeeId)
        {
            var db = new AppDbContext();

            var dishesWithoutCooker = db.DishInOrders
                                    .Where(dishInOrder => !dishInOrder.Executives.Any(executive =>
                                     executive.Employee.EmployeeType.TypeName == "Kucharz"))
                                    .OrderBy(dishInOrder => dishInOrder.Id)
                                    .ToList();


            if(dishesWithoutCooker.Count()>0)
            {
                var dishIdRedyToAssign = dishesWithoutCooker[0].Id;

                var newExecutive = new Executive
                {
                    EmployeeId = employeeId.employeeId,
                    DishInOrderId = dishIdRedyToAssign
                };

                db.Executives.Add(newExecutive);
                db.SaveChanges();

                var assignedDishWhole = db.Dishes.Find(dishesWithoutCooker[0].DishId);

                var assignedDish = new CookerDish
                {
                    Id = dishIdRedyToAssign,
                    Name = assignedDishWhole.Name
                };

                return Ok(assignedDish);
            }

            return NotFound();
        }

        [HttpPut]
        public IActionResult UpdateWaiterCheck(IdInfo2 dishToConfirm)
        {
            if (ModelState.IsValid)
            {
                var db = new AppDbContext();

                var update = db.DishInOrders.Find(dishToConfirm.dishInOrderId);

                update.KitchenCheck = true;
                db.SaveChanges();

                return Ok();
            }
            else return BadRequest();
        }

    }
}
