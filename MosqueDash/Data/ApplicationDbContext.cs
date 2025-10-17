using Microsoft.EntityFrameworkCore;
using MosqueDash.Data.Models;

namespace MosqueDash.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // This ensures the relationship between Enrollment and Attendance
            // is simple and uses EnrollmentId as the Foreign Key, which fixes the error.
            // هذا الكود يضمن أن العلاقة بين التسجيل والحضور بسيطة وتستخدم
            // EnrollmentId كمفتاح أجنبي، مما يصلح الخطأ.
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Enrollment)
                .WithMany(e => e.Attendances)
                .HasForeignKey(a => a.EnrollmentId)
                .IsRequired();

            // The relationships for Student and Course to Enrollment are automatically
            // handled by EF Core because the models are set up correctly.
            // We don't need to define them explicitly here.
            // العلاقات الأخرى يتم تعريفها تلقائيًا بواسطة النظام.
        }
    }
}

