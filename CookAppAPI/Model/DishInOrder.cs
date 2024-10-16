using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookAppAPI.Model
{
    public class DishInOrder
    {
        public DishInOrder() 
        {
            Executives = new HashSet<Executive>();
        }
        [Key]
        public int Id { get; set; }
        [ForeignKey("Dish")]
        public int DishId { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public bool KitchenCheck { get; set; }
        public bool WaiterCheck { get; set; }
        public virtual ICollection<Executive> Executives { get; set; }
        public Dish Dish { get; set; }
        public Order Order { get; set; }
    }
}
