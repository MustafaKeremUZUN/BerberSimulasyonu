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
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
            
            ViewBag.Services = await _context.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.Category)
                .ThenBy(s => s.Name)
                .ToListAsync();

            return View();
        }

        // POST: Appointment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,ServiceId,AppointmentDate,Notes")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                // Get customer and service information
                var customer = await _context.Customers.FindAsync(appointment.CustomerId);
                var service = await _context.Services.FindAsync(appointment.ServiceId);

                if (customer == null || service == null)
                {
                    ModelState.AddModelError("", "Müşteri veya hizmet bulunamadı.");
                    await LoadViewBagData();
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
        }
    }
}
