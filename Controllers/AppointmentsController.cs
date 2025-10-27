using Clinic_Appointment_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic_Appointment_System.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ClinicDbContext _context;

        public AppointmentsController(ClinicDbContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            try
            {
                var appointments = await _context.Appointments
                    .Include(a => a.Doctor)
                    .Include(a => a.Patient)
                .OrderByDescending(a => a.AppointmentDateTime)
                    .ToListAsync();
                return View(appointments);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AppointmentsController.Index: " + ex.Message + "\nStackTrace: " + ex.StackTrace);
                return StatusCode(500, "An error occurred while retrieving appointments. Please try again later.");
            }
        }

        // GET: Appointments/ByDoctor/5
        public async Task<IActionResult> ByDoctor(int? id)
        {
            if (id == null) return NotFound();
            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == id)
                .ToListAsync();
            ViewBag.Doctor = await _context.Doctors.FindAsync(id);
            return View("Index", appointments);
        }

        // GET: Appointments/ByPatient/5
        public async Task<IActionResult> ByPatient(int? id)
        {
            if (id == null) return NotFound();
            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.PatientId == id)
                .ToListAsync();
            ViewBag.Patient = await _context.Patients.FindAsync(id);
            return View("Index", appointments);
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewBag.Doctors = new SelectList(_context.Doctors, "DoctorId", "Name");
            ViewBag.Patients = new SelectList(_context.Patients, "PatientId", "Name");
            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorId,PatientId,AppointmentDateTime,Status,Notes,Prescription")] Appointment appointment)
        {
            Console.WriteLine("AppointmentsController.Create called with: " +
                $"DoctorId={appointment.DoctorId}, PatientId={appointment.PatientId}, " +
                $"AppointmentDateTime={appointment.AppointmentDateTime}, Status={appointment.Status}, " +
                $"Notes={appointment.Notes}, Prescription={appointment.Prescription}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid in Appointment Create");
                ViewBag.Doctors = new SelectList(_context.Doctors, "DoctorId", "Name", appointment.DoctorId);
                ViewBag.Patients = new SelectList(_context.Patients, "PatientId", "Name", appointment.PatientId);
                return View(appointment);
            }

            // Check if DoctorId exists
            if (!_context.Doctors.Any(d => d.DoctorId == appointment.DoctorId))
            {
                Console.WriteLine("Invalid DoctorId: " + appointment.DoctorId);
                ModelState.AddModelError("DoctorId", "Invalid Doctor selected.");
                ViewBag.Doctors = new SelectList(_context.Doctors, "DoctorId", "Name", appointment.DoctorId);
                ViewBag.Patients = new SelectList(_context.Patients, "PatientId", "Name", appointment.PatientId);
                return View(appointment);
            }

            // Check if PatientId exists
            if (!_context.Patients.Any(p => p.PatientId == appointment.PatientId))
            {
                Console.WriteLine("Invalid PatientId: " + appointment.PatientId);
                ModelState.AddModelError("PatientId", "Invalid Patient selected.");
                ViewBag.Doctors = new SelectList(_context.Doctors, "DoctorId", "Name", appointment.DoctorId);
                ViewBag.Patients = new SelectList(_context.Patients, "PatientId", "Name", appointment.PatientId);
                return View(appointment);
            }

            // Check if appointment date is in the past (based on current date: October 10, 2025)
            if (appointment.AppointmentDateTime < new DateTime(2025, 10, 10))
            {
                ModelState.AddModelError("AppointmentDateTime", "Appointment date cannot be in the past.");
                ViewBag.Doctors = new SelectList(_context.Doctors, "DoctorId", "Name", appointment.DoctorId);
                ViewBag.Patients = new SelectList(_context.Patients, "PatientId", "Name", appointment.PatientId);
                return View(appointment);
            }

            // Check for conflicting appointments
            bool hasConflict = await _context.Appointments
                .AnyAsync(a => a.DoctorId == appointment.DoctorId
                            && a.AppointmentDateTime == appointment.AppointmentDateTime
                            && a.Status != AppointmentStatus.Canceled);
            if (hasConflict)
            {
                Console.WriteLine("Appointment conflict detected for DoctorId: " + appointment.DoctorId +
                    " at " + appointment.AppointmentDateTime);
                ModelState.AddModelError("AppointmentDateTime", "Doctor already has an appointment at this time.");
                ViewBag.Doctors = new SelectList(_context.Doctors, "DoctorId", "Name", appointment.DoctorId);
                ViewBag.Patients = new SelectList(_context.Patients, "PatientId", "Name", appointment.PatientId);
                return View(appointment);
            }

            try
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                Console.WriteLine("Appointment created successfully: " + appointment.AppointmentId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in Appointment Create: " + ex.Message + "\nStackTrace: " + ex.StackTrace);
                ModelState.AddModelError("", "An error occurred while creating the appointment: " + ex.Message);
                ViewBag.Doctors = new SelectList(_context.Doctors, "DoctorId", "Name", appointment.DoctorId);
                ViewBag.Patients = new SelectList(_context.Patients, "PatientId", "Name", appointment.PatientId);
                return View(appointment);
            }
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointment == null) return NotFound();

            ViewBag.Doctors = new SelectList(_context.Doctors, "DoctorId", "Name", appointment.DoctorId);
            ViewBag.Patients = new SelectList(_context.Patients, "PatientId", "Name", appointment.PatientId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,DoctorId,PatientId,AppointmentDateTime,Status,Notes,Prescription")] Appointment appointment)
        {
            Console.WriteLine("AppointmentsController.Edit called with: " +
                $"AppointmentId={appointment.AppointmentId}, DoctorId={appointment.DoctorId}, PatientId={appointment.PatientId}, " +
                $"AppointmentDateTime={appointment.AppointmentDateTime}, Status={appointment.Status}, " +
                $"Notes={appointment.Notes}, Prescription={appointment.Prescription}");

            if (id != appointment.AppointmentId)
            {
                Console.WriteLine("Invalid ID: " + id);
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid in Appointment Edit");
                ViewBag.Doctors = new SelectList(_context.Doctors, "DoctorId", "Name", appointment.DoctorId);
                ViewBag.Patients = new SelectList(_context.Patients, "PatientId", "Name", appointment.PatientId);
                return View(appointment);
            }

            // Check if DoctorId exists
            if (!_context.Doctors.Any(d => d.DoctorId == appointment.DoctorId))
            {
                Console.WriteLine("Invalid DoctorId: " + appointment.DoctorId);
                ModelState.AddModelError("DoctorId", "Invalid Doctor selected.");
                ViewBag.Doctors = new SelectList(_context.Doctors, "DoctorId", "Name", appointment.DoctorId);
                ViewBag.Patients = new SelectList(_context.Patients, "PatientId", "Name", appointment.PatientId);
                return View(appointment);
            }

            // Check if PatientId exists
            if (!_context.Patients.Any(p => p.PatientId == appointment.PatientId))
            {
                Console.WriteLine("Invalid PatientId: " + appointment.PatientId);
                ModelState.AddModelError("PatientId", "Invalid Patient selected.");
                ViewBag.Doctors = new SelectList(_context.Doctors, "DoctorId", "Name", appointment.DoctorId);
                ViewBag.Patients = new SelectList(_context.Patients, "PatientId", "Name", appointment.PatientId);
                return View(appointment);
            }

            //// Check if appointment date is in the past (based on current date: October 10, 2025)
            //if (appointment.AppointmentDateTime < new DateTime(2025, 10, 10))
            //{
            //    ModelState.AddModelError("AppointmentDateTime", "Appointment date cannot be in the past.");
            //    ViewBag.Doctors = new SelectList(_context.Doctors, "DoctorId", "Name", appointment.DoctorId);
            //    ViewBag.Patients = new SelectList(_context.Patients, "PatientId", "Name", appointment.PatientId);
            //    return View(appointment);
            //}

            // Check for conflicting appointments
            bool hasConflict = await _context.Appointments
                .AnyAsync(a => a.DoctorId == appointment.DoctorId
                            && a.AppointmentDateTime == appointment.AppointmentDateTime
                            && a.Status != AppointmentStatus.Canceled
                            && a.AppointmentId != appointment.AppointmentId);
            if (hasConflict)
            {
                Console.WriteLine("Appointment conflict detected for DoctorId: " + appointment.DoctorId +
                    " at " + appointment.AppointmentDateTime);
                ModelState.AddModelError("AppointmentDateTime", "Doctor already has an appointment at this time.");
                ViewBag.Doctors = new SelectList(_context.Doctors, "DoctorId", "Name", appointment.DoctorId);
                ViewBag.Patients = new SelectList(_context.Patients, "PatientId", "Name", appointment.PatientId);
                return View(appointment);
            }

            try
            {

                var existingAppointment = await _context.Appointments
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.AppointmentId == id);

                if (existingAppointment == null)
                {
                    return NotFound();
                }


                appointment.Status = existingAppointment.Status;

                _context.Update(appointment);
                await _context.SaveChangesAsync();

                Console.WriteLine("Appointment updated successfully: " + appointment.AppointmentId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in Appointment Edit: " + ex.Message + "\nStackTrace: " + ex.StackTrace);
                if (!_context.Appointments.Any(e => e.AppointmentId == appointment.AppointmentId))
                {
                    return NotFound();
                }
                ModelState.AddModelError("", "An error occurred while updating the appointment: " + ex.Message);
                ViewBag.Doctors = new SelectList(_context.Doctors, "DoctorId", "Name", appointment.DoctorId);
                ViewBag.Patients = new SelectList(_context.Patients, "PatientId", "Name", appointment.PatientId);
                return View(appointment);
            }
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}