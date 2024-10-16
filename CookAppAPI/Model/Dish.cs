using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookAppAPI.Model
{
    public class Dish
    {
        public Dish() 
        {
            DishInOrders = new HashSet<DishInOrder>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public virtual ICollection<DishInOrder> DishInOrders { get; set; }
    }
}
