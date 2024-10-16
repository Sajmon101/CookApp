using System;
using System.Collections.Generic;

namespace CookApp.DB;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public int EmployeeTypeId { get; set; }

    public virtual EmployeeType EmployeeType { get; set; } = null!;

    public virtual ICollection<Executive> Executives { get; set; } = new List<Executive>();

    public virtual Login? Login { get; set; }
}
