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

            var cascadeFKs = builder.Model.GetEntityTypes()
                    .SelectMany(t => t.GetForeignKeys())
                    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;


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
