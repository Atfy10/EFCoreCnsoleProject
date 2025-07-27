using System;
using System.Collections.Generic;

namespace EjadaEFProject.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public virtual ICollection<StdCr> StdCrs { get; set; } = new List<StdCr>();
}
