using System;
using System.Collections.Generic;
using EjadaEFProject.Models;
using Microsoft.EntityFrameworkCore;

namespace EjadaEFProject.Data;

public partial class EduDbContext : DbContext
{
    public EduDbContext()
    {
    }

    public EduDbContext(DbContextOptions<EduDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<StdCr> StdCrs { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=Study;trusted_connection=true;trustservercertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Course");

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Did);

            entity.ToTable("Department");

            entity.Property(e => e.Did).HasColumnName("DId");
            entity.Property(e => e.Daddress).HasColumnName("DAddress");
            entity.Property(e => e.Dname).HasColumnName("DName");
        });

        modelBuilder.Entity<StdCr>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.CourseId });

            entity.HasIndex(e => e.CourseId, "IX_StdCrs_CourseId");

            entity.HasOne(d => d.Course).WithMany(p => p.StdCrs).HasForeignKey(d => d.CourseId);

            entity.HasOne(d => d.Student).WithMany(p => p.StdCrs).HasForeignKey(d => d.StudentId);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasIndex(e => e.DeptId, "IX_Students_DeptId");

            entity.HasOne(d => d.Dept).WithMany(p => p.Students).HasForeignKey(d => d.DeptId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
