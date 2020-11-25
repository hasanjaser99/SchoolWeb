using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolWeb.Models;

namespace SchoolWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CourseTeachers>().HasKey(i => new { i.TeacherId, i.CourseId });

            builder.Entity<Section>()
            .HasMany(s => s.Students)
            .WithOne(s => s.Section)
            .OnDelete(DeleteBehavior.Restrict);



            builder.Entity<Teacher>()
            .HasMany(t => t.Classes)
            .WithOne(c => c.Teacher)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Teacher>()
            .HasMany(t => t.Sections)
            .WithOne(s => s.Teacher)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StudentFee>()
            .HasMany(s => s.MonthlyPayments)
            .WithOne(m => m.StudentFee)
            .OnDelete(DeleteBehavior.Cascade);


        }


        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<CourseTeachers> CourseTeachers { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<StudentFee> StudentFees { get; set; }
        public DbSet<SchoolFee> SchoolFees { get; set; }
        public DbSet<MonthlyPayment> MonthlyPayments { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsImages> NewsImages { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityImages> ActivityImages { get; set; }
        public DbSet<SuggestionComplain> SuggestionComplains { get; set; }


    }
}
