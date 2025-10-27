using System.ComponentModel.DataAnnotations;

namespace Clinic_Appointment_System.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        public DateTime? BirthDate { get; set; }

        public int Age => BirthDate.HasValue ? DateTime.Now.Year - BirthDate.Value.Year - (DateTime.Now.DayOfYear < BirthDate.Value.DayOfYear ? 1 : 0) : 0;

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        //public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    }

}
