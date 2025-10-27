using Clinic_Appointment_System.Models;
using Clinic_Appointment_System.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Clinic_Appointment_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ClinicDbContext _context;
        private const string AdminPassword = "admin123"; 

        public HomeController(ILogger<HomeController> logger, ClinicDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: Home/Index 
        public async Task<IActionResult> Index()
        {
             
            if (HttpContext.Session.GetString("IsLoggedIn") != "true")
            {
                ViewBag.ShowLoginModal = true;
                return View(new DashboardViewModel());  
            }

 
            var viewModel = new DashboardViewModel
            {
                DoctorCount = await _context.Doctors.CountAsync(),
                PatientCount = await _context.Patients.CountAsync(),
                UpcomingAppointmentCount = await _context.Appointments
                    .Where(a => a.AppointmentDateTime > DateTime.Now && a.Status == AppointmentStatus.Scheduled)
                    .CountAsync()
            };

            return View(viewModel);
        }

        // POST: Home/Login  
        [HttpPost]
        public IActionResult Login(string password)
        {
            if (password == AdminPassword)
            {
                HttpContext.Session.SetString("IsLoggedIn", "true");
                return RedirectToAction("Index"); 
            }
            else
            {
                ViewBag.ShowLoginModal = true;
                ViewBag.LoginError = "Invalid password. Please try again.";
                return View("Index", new DashboardViewModel());  
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}