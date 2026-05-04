using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BerberSimulasyonu.Data;
using BerberSimulasyonu.Models;

namespace BerberSimulasyonu.Controllers
{
    public class CustomerController : Controller
    {
        private readonly BerberDbContext _context;

        public CustomerController(BerberDbContext context)
        {
            _context = context;
        }

        // GET: Customer/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Customer/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("FirstName,LastName,PhoneNumber,Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                // Check if phone number already exists
                var existingCustomer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.PhoneNumber == customer.PhoneNumber);
                
                if (existingCustomer != null)
                {
                    ModelState.AddModelError("PhoneNumber", "Bu telefon numarası zaten kayıtlı.");
                    return View(customer);
                }

                customer.CreatedDate = DateTime.Now;
                customer.VisitCount = 0;
                
                _context.Add(customer);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Müşteri başarıyla kaydedildi!";
                return RedirectToAction(nameof(Details), new { id = customer.Id });
            }
            return View(customer);
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Appointments)
                .ThenInclude(a => a.Service)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customer/List
        public async Task<IActionResult> List()
        {
            var customers = await _context.Customers
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
            return View(customers);
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,PhoneNumber,Email")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCustomer = await _context.Customers.FindAsync(id);
                    if (existingCustomer == null)
                    {
                        return NotFound();
                    }

                    // Check if phone number is being changed and if it already exists
                    var phoneCheck = await _context.Customers
                        .FirstOrDefaultAsync(c => c.PhoneNumber == customer.PhoneNumber && c.Id != id);
                    
                    if (phoneCheck != null)
                    {
                        ModelState.AddModelError("PhoneNumber", "Bu telefon numarası başka bir müşteri tarafından kullanılıyor.");
                        return View(customer);
                    }

                    existingCustomer.FirstName = customer.FirstName;
                    existingCustomer.LastName = customer.LastName;
                    existingCustomer.PhoneNumber = customer.PhoneNumber;
                    existingCustomer.Email = customer.Email;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Success"] = "Müşteri bilgileri başarıyla güncellendi!";
                return RedirectToAction(nameof(Details), new { id = customer.Id });
            }
            return View(customer);
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
