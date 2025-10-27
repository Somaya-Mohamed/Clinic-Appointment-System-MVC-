using System.ComponentModel.DataAnnotations;

namespace Clinic_Appointment_System.Models
{
    public enum AppointmentStatus
    {
        Scheduled,
        Completed,
        Canceled
    }

    public class Appointment
    {
        public int AppointmentId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Appointment date and time is required")]
        public DateTime AppointmentDateTime { get; set; }

        [Required]
        public AppointmentStatus Status { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public string? Prescription { get; set; } 

        public Doctor? Doctor { get; set; }
        public Patient? Patient { get; set; }
    }

}
