using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookAppAPI.Model
{
    public class Login
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public string EmployeeLogin {  get; set; }
        public string Password { get; set; }
        public Employee Employee { get; set; }

    }
}
