using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookAppAPI.Model
{
    public class Order
    {
        public Order() 
        {
            DishInOrders = new HashSet<DishInOrder>();
        }
        public int Id { get; set; }
        public int TableNum { get; set; }
        public DateTime Date { get; set; }
        public bool IsArch {  get; set; }
        public virtual ICollection<DishInOrder> DishInOrders { get; set; }
    }
}
