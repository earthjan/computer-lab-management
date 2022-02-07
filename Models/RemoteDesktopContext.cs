using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace RemoteScreenshot.Models
{
    public partial class RemoteDesktopContext : DbContext
    {
        public RemoteDesktopContext()
        {
        }

        public RemoteDesktopContext(DbContextOptions<RemoteDesktopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdminAccount> AdminAccounts { get; set; }
        public virtual DbSet<AdminConfig> AdminConfigs { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<AttendanceStudent> AttendanceStudents { get; set; }
        public virtual DbSet<Desktop> Desktops { get; set; }
        public virtual DbSet<Laboratory> Laboratories { get; set; }
        public virtual DbSet<LaboratoryDesktop> LaboratoryDesktops { get; set; }
        public virtual DbSet<Screenshot> Screenshots { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminConfig>(entity =>
            {
                entity.Property(e => e.ScreenshotDirectory).HasComment("from the server machine, this is where all the screenshots of lab desktops are pointed by desktops.screenshot_directory. So, make sure that all fields of desktops.screenshot_directory are pointing correctly.");
            });

            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValueSql("'1'");
            });

            modelBuilder.Entity<AttendanceStudent>(entity =>
            {
                entity.HasOne(d => d.Attendance)
                    .WithMany(p => p.AttendanceStudents)
                    .HasForeignKey(d => d.AttendanceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("attendance_id_attendance_students");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.AttendanceStudents)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("student_id_attendance_students");
            });

            modelBuilder.Entity<Desktop>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValueSql("'1'");
            });

            modelBuilder.Entity<LaboratoryDesktop>(entity =>
            {
                entity.HasOne(d => d.Desktop)
                    .WithMany(p => p.LaboratoryDesktops)
                    .HasForeignKey(d => d.DesktopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("desktop_id_laboratory_desktops");

                entity.HasOne(d => d.Laboratory)
                    .WithMany(p => p.LaboratoryDesktops)
                    .HasForeignKey(d => d.LaboratoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("laboratory_id_laboratory_desktops");
            });

            modelBuilder.Entity<Screenshot>(entity =>
            {
                entity.HasOne(d => d.Desktop)
                    .WithMany(p => p.Screenshots)
                    .HasForeignKey(d => d.DesktopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("desktop_id_screenshots");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
