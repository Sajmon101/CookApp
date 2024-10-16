using System;
using System.Collections.Generic;

namespace CookApp.DB;

public partial class Order
{
    public int Id { get; set; }

    public int TableNum { get; set; }

    public DateTime Date { get; set; }

    public bool IsArch { get; set; }

    public virtual ICollection<DishInOrder> DishInOrders { get; set; } = new List<DishInOrder>();
}
