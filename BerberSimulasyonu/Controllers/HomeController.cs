using System.Diagnostics;
using BerberSimulasyonu.Models;
using Microsoft.AspNetCore.Mvc;
using BerberSimulasyonu.Data;
using Microsoft.EntityFrameworkCore;

namespace BerberSimulasyonu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BerberDbContext _context;

        public HomeController(ILogger<HomeController> logger, BerberDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dashboard = new DashboardViewModel
            {
                TotalCustomers = await _context.Customers.CountAsync(),
                TotalAppointments = await _context.Appointments.CountAsync(),
                CompletedAppointments = await _context.Appointments.CountAsync(a => a.IsCompleted),
                PendingAppointments = await _context.Appointments.CountAsync(a => !a.IsCompleted),
                TotalRevenue = await _context.Appointments.Where(a => a.IsCompleted).SumAsync(a => a.FinalPrice),
                RecentAppointments = await _context.Appointments
                    .Include(a => a.Customer)
                    .Include(a => a.Service)
                    .OrderByDescending(a => a.AppointmentDate)
                    .Take(5)
                    .ToListAsync(),
                TopCustomers = await _context.Customers
                    .OrderByDescending(c => c.VisitCount)
                    .Take(5)
                    .ToListAsync()
            };

            return View(dashboard);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class DashboardViewModel
    {
        public int TotalCustomers { get; set; }
        public int TotalAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public int PendingAppointments { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<Appointment> RecentAppointments { get; set; } = new();
        public List<Customer> TopCustomers { get; set; } = new();
    }
}
