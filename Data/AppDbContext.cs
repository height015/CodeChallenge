using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Domain;

namespace CodeChallenge.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }


    public DbSet<Courses> Course { get; set; }

    public DbSet<Grade> Grade { get; set; }

    public DbSet<Student> Students { get; set; }

    public DbSet<StudentCourse> StudentCourses { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);



    }

}


