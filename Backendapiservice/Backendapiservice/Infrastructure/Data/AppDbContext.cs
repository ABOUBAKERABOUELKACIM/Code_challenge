using Backendapiservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backendapiservice.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Office> Offices { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Assistant> Assistants { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User base configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            });

            // Admin configuration (inherits base User properties)
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.Property(e => e.Role).HasMaxLength(50);
            });

            // Office configuration
            modelBuilder.Entity<Office>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.Phone).HasMaxLength(20);
            });

            // Doctor configuration (inherits from User)
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.Property(e => e.Specialization).HasMaxLength(200);

                entity.HasOne(e => e.Office)
                      .WithMany(o => o.Doctors)
                      .HasForeignKey(e => e.OfficeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Assistant configuration (inherits from User)
            modelBuilder.Entity<Assistant>(entity =>
            {
                entity.HasOne(e => e.Doctor)
                      .WithMany(d => d.Assistants)
                      .HasForeignKey(e => e.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Patient configuration
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Phone).HasMaxLength(20);
            });

            // Appointment configuration
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Notes).HasMaxLength(1000);

                entity.HasOne(e => e.Doctor)
                      .WithMany(d => d.Appointments)
                      .HasForeignKey(e => e.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Patient)
                      .WithMany(p => p.Appointments)
                      .HasForeignKey(e => e.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.CreatedByAssistant)
                      .WithMany(a => a.CreatedAppointments)
                      .HasForeignKey(e => e.CreatedByAssistantId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}