using System.ComponentModel.DataAnnotations;

namespace Clinic_Appointment_System.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Specialization is required")]
        [StringLength(100)]
        public string Specialization { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }

}

