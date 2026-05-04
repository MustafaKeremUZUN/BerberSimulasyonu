using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BerberSimulasyonu.Data;
using BerberSimulasyonu.Models;

namespace BerberSimulasyonu.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly BerberDbContext _context;

        public AppointmentController(BerberDbContext context)
        {
            _context = context;
        }

        // GET: Appointment/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Customers = await _context.Customers
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToListAsync();
            
            ViewBag.Services = await _context.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.Category)
                .ThenBy(s => s.Name)
                .ToListAsync();
            
            ViewBag.Employees = await _context.Employees
                .Where(e => e.IsActive)
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToListAsync();
            
            return View();
        }

        // POST: Appointment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,ServiceId,EmployeeId,AppointmentDate,Notes")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                // Get customer, service and employee information
                var customer = await _context.Customers.FindAsync(appointment.CustomerId);
                var service = await _context.Services.FindAsync(appointment.ServiceId);
                var employee = await _context.Employees.FindAsync(appointment.EmployeeId);

                if (customer == null || service == null || employee == null)
                {
                    ModelState.AddModelError("", "Müşteri, hizmet veya çalışan bulunamadı.");
                    ViewBag.Customers = await _context.Customers.ToListAsync();
                    ViewBag.Services = await _context.Services.Where(s => s.IsActive).ToListAsync();
                    ViewBag.Employees = await _context.Employees.Where(e => e.IsActive).ToListAsync();
                    return View(appointment);
                }

                // Check employee availability
                if (!await IsEmployeeAvailable(appointment.EmployeeId, appointment.AppointmentDate))
                {
                    ModelState.AddModelError("", "Seçilen çalışan bu saatte müsait değil. Lütfen başka bir çalışan veya saat seçin.");
                    ViewBag.Customers = await _context.Customers.ToListAsync();
                    ViewBag.Services = await _context.Services.Where(s => s.IsActive).ToListAsync();
                    ViewBag.Employees = await _context.Employees.Where(e => e.IsActive).ToListAsync();
                    return View(appointment);
                }

                // Calculate pricing and discount
                decimal discountPercentage = CalculateDiscountPercentage(customer.VisitCount);
                decimal discountAmount = service.Price * (discountPercentage / 100);
                decimal finalPrice = service.Price - discountAmount;

                // Set appointment details
                appointment.OriginalPrice = service.Price;
                appointment.DiscountPercentage = discountPercentage;
                appointment.DiscountAmount = discountAmount;
                appointment.FinalPrice = finalPrice;
                appointment.IsCompleted = false;

                // Add appointment
                _context.Appointments.Add(appointment);
                
                // Update customer visit count
                customer.VisitCount += 1;
                
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Randevu başarıyla oluşturuldu! İndirim: %{discountPercentage}";
                return RedirectToAction(nameof(Details), new { id = appointment.Id });
            }

            await LoadViewBagData();
            return View(appointment);
        }

        // GET: Appointment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointment/List
        public async Task<IActionResult> List()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Service)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
            return View(appointments);
        }

        // GET: Appointment/Complete/5
        public async Task<IActionResult> Complete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.IsCompleted = true;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Randevu tamamlandı olarak işaretlendi.";
            return RedirectToAction(nameof(Details), new { id = appointment.Id });
        }

        // GET: Appointment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                // Decrease customer visit count
                var customer = await _context.Customers.FindAsync(appointment.CustomerId);
                if (customer != null && customer.VisitCount > 0)
                {
                    customer.VisitCount -= 1;
                }

                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Randevu başarıyla silindi.";
            return RedirectToAction(nameof(List));
        }

        // Helper method to calculate discount percentage
        private decimal CalculateDiscountPercentage(int visitCount)
        {
            if (visitCount >= 31)
                return 20;
            else if (visitCount >= 11)
                return 10;
            else
                return 0;
        }

        // Helper method to load ViewBag data
        private async Task LoadViewBagData()
        {
            ViewBag.Customers = await _context.Customers
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
            
            ViewBag.Services = await _context.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.Category)
                .ThenBy(s => s.Name)
                .ToListAsync();
            
            ViewBag.Employees = await _context.Employees
                .Where(e => e.IsActive)
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToListAsync();
        }

        // Helper method to check employee availability
        private async Task<bool> IsEmployeeAvailable(int employeeId, DateTime appointmentDate)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null || !employee.IsActive) return false;

            // Check working hours
            var appointmentTime = appointmentDate.TimeOfDay;
            if (appointmentTime < employee.DefaultWorkStart || appointmentTime > employee.DefaultWorkEnd)
            {
                return false;
            }

            // Check existing appointments
            var existingAppointments = await _context.Appointments
                .Where(a => a.EmployeeId == employeeId && 
                           a.AppointmentDate.Date == appointmentDate.Date &&
                           !a.IsCompleted)
                .ToListAsync();

            // Average appointment duration (30 minutes)
            var averageAppointmentDuration = 30;

            foreach (var appointment in existingAppointments)
            {
                var appointmentStartTime = appointment.AppointmentDate;
                var appointmentEndTime = appointmentStartTime.AddMinutes(averageAppointmentDuration);

                // Check for conflicts
                if (appointmentDate >= appointmentStartTime && appointmentDate < appointmentEndTime)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
