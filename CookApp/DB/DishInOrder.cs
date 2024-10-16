using System;
using System.Collections.Generic;

namespace CookApp.DB;

public partial class DishInOrder
{
    public int Id { get; set; }

    public int DishId { get; set; }

    public int OrderId { get; set; }

    public bool KitchenCheck { get; set; }

    public bool WaiterCheck { get; set; }

    public virtual ICollection<Executive> Executives { get; set; } = new List<Executive>();
    
    public virtual Dish Dish { get; set; } = null!;
    
    public virtual Order Order { get; set; } = null!;
}
