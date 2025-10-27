using Clinic_Appointment_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clinic_Appointment_System.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ClinicDbContext _context;

        public DoctorsController(ClinicDbContext context)
        {
            _context = context;
        }

        // GET: Doctors
        public async Task<IActionResult> Index(string specialization = null)
        {
            Console.WriteLine("DoctorsController.Index called");
            var doctors = _context.Doctors.AsQueryable();

            if (!string.IsNullOrEmpty(specialization))
            {
                doctors = doctors.Where(d => d.Specialization == specialization);
                ViewData["CurrentSpecialization"] = specialization; 
            }

            ViewBag.Specializations = await _context.Doctors.Select(d => d.Specialization).Distinct().ToListAsync();

            return View(await doctors.ToListAsync());
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Console.WriteLine("DoctorsController.Details called");
            if (id == null) return NotFound();
            var doctor = await _context.Doctors.FirstOrDefaultAsync(m => m.DoctorId == id);
            if (doctor == null) return NotFound();
            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            Console.WriteLine("DoctorsController.Create GET called");
            return View();
        }

        // POST: Doctors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Doctor doctor)
        {
            Console.WriteLine("DoctorsController.Create called with: " +
                $"Name={doctor.Name ?? "NULL"}, Specialization={doctor.Specialization ?? "NULL"}, Phone={doctor.Phone ?? "NULL"}, Appointments={(doctor.Appointments != null ? doctor.Appointments.Count : "NULL")}");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors.Select(e => new { x.Key, Error = e }))
                                      .Select(e => $"{e.Key}: {e.Error.ErrorMessage}");
                Console.WriteLine("ModelState Errors in Doctor Create: " + string.Join(", ", errors));
                return View(doctor);
            }

            try
            {
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                Console.WriteLine("Doctor saved successfully: " + doctor.DoctorId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in Doctor Create: " + ex.Message + "\nStackTrace: " + ex.StackTrace);
                ModelState.AddModelError("", "An error occurred while saving the doctor: " + ex.Message);
                return View(doctor);
            }
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return NotFound();
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DoctorId,Name,Specialization,Phone")] Doctor doctor)
        {
            Console.WriteLine("DoctorsController.Edit called with: " +
                $"DoctorId={doctor.DoctorId}, Name={doctor.Name}, Specialization={doctor.Specialization}, Phone={doctor.Phone}");

            if (id != doctor.DoctorId)
            {
                Console.WriteLine("Invalid ID: " + id);
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Console.WriteLine("ModelState Errors in Doctor Edit: " + string.Join(", ", errors));
                return View(doctor);
            }

            try
            {
                _context.Update(doctor);
                await _context.SaveChangesAsync();
                Console.WriteLine("Doctor updated successfully: " + doctor.DoctorId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in Doctor Edit: " + ex.Message + "\nStackTrace: " + ex.StackTrace);
                if (!_context.Doctors.Any(e => e.DoctorId == doctor.DoctorId))
                {
                    return NotFound();
                }
                ModelState.AddModelError("", "An error occurred while updating the doctor: " + ex.Message);
                return View(doctor);
            }
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var doctor = await _context.Doctors.FirstOrDefaultAsync(m => m.DoctorId == id);
            if (doctor == null) return NotFound();
            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null) _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}