using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookAppAPI.Model
{
    public class Employee
    {
        public Employee()
        {
            Executives = new HashSet<Executive>();
        }

        [Key]
        public int EmployeeId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        [ForeignKey("EmployeeTypes")]
        public int EmployeeTypesId;
        public virtual ICollection<Executive> Executives { get; set; }
        public required EmployeeType EmployeeType { get; set; }
        public Login Login {  get; set; } 
    }
}
