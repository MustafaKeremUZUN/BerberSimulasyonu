using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BerberSimulasyonu.Data;
using BerberSimulasyonu.Models;

namespace BerberSimulasyonu.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly BerberDbContext _context;

        public EmployeeController(BerberDbContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees
                .Include(e => e.Appointments)
                .Where(e => e.IsActive)
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToListAsync();
            
            return View(employees);
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Appointments)
                    .ThenInclude(a => a.Customer)
                .Include(e => e.Appointments)
                    .ThenInclude(a => a.Service)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,PhoneNumber,Email,Position,HourlyRate,HireDate,DefaultWorkStart,DefaultWorkEnd")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.IsActive = true;
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,PhoneNumber,Email,Position,HourlyRate,HireDate,DefaultWorkStart,DefaultWorkEnd,IsActive")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                employee.IsActive = false; // Soft delete
                _context.Update(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Employee/Schedule/5
        public async Task<IActionResult> Schedule(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Appointments)
                    .ThenInclude(a => a.Customer)
                .Include(e => e.Appointments)
                    .ThenInclude(a => a.Service)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            // Çalışanın bugünkü işe başlama ve bitiş saatlerini ayarla
            var today = DateTime.Today;
            if (employee.WorkStartTime?.Date != today)
            {
                employee.WorkStartTime = today.Add(employee.DefaultWorkStart);
            }
            if (employee.WorkEndTime?.Date != today)
            {
                employee.WorkEndTime = today.Add(employee.DefaultWorkEnd);
            }

            return View(employee);
        }

        // POST: Employee/CheckIn/5
        [HttpPost]
        public async Task<IActionResult> CheckIn(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                employee.WorkStartTime = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Schedule), new { id = id });
        }

        // POST: Employee/CheckOut/5
        [HttpPost]
        public async Task<IActionResult> CheckOut(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                employee.WorkEndTime = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Schedule), new { id = id });
        }

        // GET: Employee/AvailableEmployees
        public async Task<IActionResult> AvailableEmployees(DateTime appointmentDate)
        {
            var employees = await _context.Employees
                .Where(e => e.IsActive)
                .ToListAsync();

            var availableEmployees = new List<object>();

            foreach (var employee in employees)
            {
                // Çalışanın o saatte müsait olup olmadığını kontrol et
                var isAvailable = await IsEmployeeAvailable(employee.Id, appointmentDate);
                
                availableEmployees.Add(new
                {
                    Id = employee.Id,
                    Name = employee.FullName,
                    Position = employee.Position,
                    IsAvailable = isAvailable,
                    WorkStart = employee.DefaultWorkStart,
                    WorkEnd = employee.DefaultWorkEnd
                });
            }

            return Json(availableEmployees);
        }

        private async Task<bool> IsEmployeeAvailable(int employeeId, DateTime appointmentDate)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null || !employee.IsActive) return false;

            // Çalışma saatleri kontrolü
            var appointmentTime = appointmentDate.TimeOfDay;
            if (appointmentTime < employee.DefaultWorkStart || appointmentTime > employee.DefaultWorkEnd)
            {
                return false;
            }

            // Mevcut randevuları kontrol et
            var existingAppointments = await _context.Appointments
                .Where(a => a.EmployeeId == employeeId && 
                           a.AppointmentDate.Date == appointmentDate.Date &&
                           !a.IsCompleted)
                .ToListAsync();

            // Her randevu için ortalama süre (dakika)
            var averageAppointmentDuration = 30; // 30 dakika

            foreach (var appointment in existingAppointments)
            {
                var appointmentStartTime = appointment.AppointmentDate;
                var appointmentEndTime = appointmentStartTime.AddMinutes(averageAppointmentDuration);

                // Yeni randevu mevcut bir randevu ile çakışıyor mu?
                if (appointmentDate >= appointmentStartTime && appointmentDate < appointmentEndTime)
                {
                    return false;
                }
            }

            return true;
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
