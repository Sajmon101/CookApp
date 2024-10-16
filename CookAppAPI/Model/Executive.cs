using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookAppAPI.Model
{
    public class Executive
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public int DishInOrderId { get; set; }
        public Employee Employee { get; set; }
        public DishInOrder DishInOrder { get; set; }
    }
}
