using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EjadaEFProject.Models;

public partial class Student
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Age { get; set; }

    public int DeptId { get; set; }

    public virtual Department Dept { get; set; } = null!;

    public virtual ICollection<StdCr> StdCrs { get; set; } = new List<StdCr>();
}
