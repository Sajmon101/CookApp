using System;
using System.Collections.Generic;

namespace CookApp.DB;

public partial class Executive
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int DishInOrderId { get; set; }

    public virtual DishInOrder DishInOrder { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
