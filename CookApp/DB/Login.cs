using System;
using System.Collections.Generic;

namespace CookApp.DB;

public partial class Login
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string EmployeeLogin { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
