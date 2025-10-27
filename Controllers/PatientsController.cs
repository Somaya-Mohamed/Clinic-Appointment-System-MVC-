using Clinic_Appointment_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Clinic_Appointment_System.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ClinicDbContext _context;

        public PatientsController(ClinicDbContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Patients.ToListAsync());
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var patient = await _context.Patients.FirstOrDefaultAsync(m => m.PatientId == id);
            if (patient == null) return NotFound();
            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,BirthDate,Phone,Address")] Patient patient)
        {
            Console.WriteLine("PatientsController.Create called with: " +
                $"Name={patient.Name}, BirthDate={patient.BirthDate}, Phone={patient.Phone}, Address={patient.Address}");


            if (!ModelState.IsValid)
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors.Select(e => new { x.Key, Error = e }))
                                      .Select(e => $"{e.Key}: {e.Error.ErrorMessage}"); Console.WriteLine("ModelState Errors in Patient Create: " + string.Join(", ", errors));
                return View(patient);
            }


            try
            {
                _context.Add(patient);
                await _context.SaveChangesAsync();
                Console.WriteLine("Patient saved successfully: " + patient.PatientId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in Patient Create: " + ex.Message + "\nStackTrace: " + ex.StackTrace);
                ModelState.AddModelError("", "An error occurred while saving the patient: " + ex.Message);
                return View(patient);
            }
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();
            return View(patient);
        }

        // POST: Patients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientId,Name,BirthDate,Phone,Address")] Patient patient)
        {
            Console.WriteLine("PatientsController.Edit called with: " +
                $"PatientId={patient.PatientId}, Name={patient.Name}, BirthDate={patient.BirthDate}, Phone={patient.Phone}, Address={patient.Address}");

            if (id != patient.PatientId)
            {
                Console.WriteLine("Invalid ID: " + id);
                return NotFound();
            }


            try
            {
                _context.Update(patient);
                await _context.SaveChangesAsync();
                Console.WriteLine("Patient updated successfully: " + patient.PatientId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in Patient Edit: " + ex.Message + "\nStackTrace: " + ex.StackTrace);
                if (!_context.Patients.Any(e => e.PatientId == patient.PatientId))
                {
                    return NotFound();
                }
                ModelState.AddModelError("", "An error occurred while updating the patient: " + ex.Message);
                return View(patient);
            }
        }



        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var patient = await _context.Patients.FirstOrDefaultAsync(m => m.PatientId == id);
            if (patient == null) return NotFound();
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null) _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Patients/History/5
        public async Task<IActionResult> History(int? id)
        {
            if (id == null) return NotFound();
            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == id)
                .OrderByDescending(a => a.AppointmentDateTime)
                .ToListAsync();
            ViewBag.Patient = await _context.Patients.FindAsync(id);
            return View(appointments);

            //return RedirectToAction(nameof(Index));
        }
    }


}
