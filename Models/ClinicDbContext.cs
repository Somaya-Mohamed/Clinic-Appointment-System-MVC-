using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Clinic_Appointment_System.Models
{

    public class ClinicDbContext : DbContext
    {
        public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Data for Doctors
            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { DoctorId = 1, Name = "د. سالي ماهر", Specialization = "باطنة", Phone = "0123456789" },
                new Doctor { DoctorId = 2, Name = "د. سارة علي", Specialization = "أطفال", Phone = "0987654321" },
                new Doctor { DoctorId = 3, Name = "د. محمد حسن", Specialization = "جراحة", Phone = "0112233445" }
            );

            // Seed Data for Patients
            modelBuilder.Entity<Patient>().HasData(
                new Patient { PatientId = 1, Name = "علي أحمد", BirthDate = new DateTime(1990, 5, 15), Phone = "0123456789", Address = "القاهرة" },
                new Patient { PatientId = 2, Name = "فاطمة خالد", BirthDate = new DateTime(1985, 3, 22), Phone = "0987654321", Address = "الجيزة" },
                new Patient { PatientId = 3, Name = "خالد محمود", BirthDate = new DateTime(2000, 7, 10), Phone = "0112233445", Address = "الإسكندرية" },
                new Patient { PatientId = 4, Name = "منى سعيد", BirthDate = new DateTime(1995, 11, 30), Phone = "0129988776", Address = "المنصورة" },
                new Patient { PatientId = 5, Name = "يوسف عمر", BirthDate = new DateTime(2010, 1, 5), Phone = "0155544332", Address = "أسيوط" }
            );

            // Seed Data for Appointments with static DateTime values
            modelBuilder.Entity<Appointment>().HasData(
                new Appointment { AppointmentId = 1, DoctorId = 1, PatientId = 1, AppointmentDateTime = new DateTime(2025, 10, 6, 10, 0, 0), Status = AppointmentStatus.Scheduled, Notes = "فحص دوري" },
                new Appointment { AppointmentId = 2, DoctorId = 2, PatientId = 2, AppointmentDateTime = new DateTime(2025, 10, 7, 14, 0, 0), Status = AppointmentStatus.Scheduled, Notes = "استشارة" },
                new Appointment { AppointmentId = 3, DoctorId = 1, PatientId = 3, AppointmentDateTime = new DateTime(2025, 10, 4, 9, 0, 0), Status = AppointmentStatus.Completed, Notes = "متابعة" }
            );
        }
    }
}
