using System;
using System.Collections.Generic;

namespace CookApp.DB;

public partial class EmployeeType
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
