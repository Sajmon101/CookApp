using CookAppAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

namespace CookAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeControler : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var db = new AppDbContext();
            var list = db.Employees.ToList();
            return Ok(list);
        }
        [HttpGet("{EmployeeId}")]
        public IActionResult Get(int EmployeeId)
        {
            var db = new AppDbContext();
            Employee? employee = db.Employees.FirstOrDefault(x => x.EmployeeId == EmployeeId);
            if (employee == null)
                return NotFound();
            else
                return Ok(employee);
        }
    }
}
