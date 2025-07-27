using System;
using System.Collections.Generic;

namespace EjadaEFProject.Models;

public partial class Department
{
    public int Did { get; set; }

    public string Dname { get; set; } = null!;

    public string Daddress { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
