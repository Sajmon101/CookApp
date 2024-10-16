using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookAppAPI.Model
{
    public class EmployeeType
    {
        public EmployeeType()
        {
            Employees = new HashSet<Employee>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string TypeName { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
