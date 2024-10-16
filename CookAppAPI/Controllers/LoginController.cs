using CookAppAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CookAppAPI.Controllers.DishInOrderController;

namespace CookAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public class LoginInfo
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        public IActionResult CheckLoginData([FromBody] LoginInfo inputInfo)
        {
            var db = new AppDbContext();

            var result = db.Logins
                .Include(login=>login.Employee)
                .Include(login=>login.Employee.EmployeeType)
                .Where(login => login.EmployeeLogin == inputInfo.UserName && login.Password == inputInfo.Password)
                .Select(login => new
                {
                    Id = login.EmployeeId,
                    TypeName = login.Employee.EmployeeType.TypeName,
                    Name = login.Employee.Name,
                    Surname = login.Employee.Surname
                }).ToList();

            if (result.Count>0) return Ok(result);
            else return NotFound();
        }
    }
}
