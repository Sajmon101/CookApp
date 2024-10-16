using System;
using System.Collections.Generic;

namespace CookApp.DB;

public partial class Dish
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public float Price { get; set; }

    public virtual ICollection<DishInOrder> DishInOrders { get; set; } = new List<DishInOrder>();
}
