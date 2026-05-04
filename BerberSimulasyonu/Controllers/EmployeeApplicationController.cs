using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BerberSimulasyonu.Data;
using BerberSimulasyonu.Models;
using System.Text.Json;

namespace BerberSimulasyonu.Controllers
{
    public class EmployeeApplicationController : Controller
    {
        private readonly BerberDbContext _context;

        public EmployeeApplicationController(BerberDbContext context)
        {
            _context = context;
        }

        // GET: EmployeeApplication
        public async Task<IActionResult> Index()
        {
            var applications = await _context.EmployeeApplications
                .Include(a => a.Employee)
                .OrderByDescending(a => a.ApplicationDate)
                .ToListAsync();
            
            return View(applications);
        }

        // GET: EmployeeApplication/Create
        public async Task<IActionResult> Create()
        {
            // Check if we already have 4 employees
            var activeEmployeesCount = await _context.Employees.CountAsync(e => e.IsActive);
            if (activeEmployeesCount >= 4)
            {
                ViewBag.ErrorMessage = "Maalesef şu anda 4 çalışanımız var ve yeni başvuru alamıyoruz.";
                return View("ApplicationClosed");
            }

            // Get 5 random questions for the quiz
            var questions = await _context.QuizQuestions
                .Where(q => q.IsActive)
                .OrderBy(q => Guid.NewGuid()) // Random order
                .Take(5)
                .ToListAsync();
            
            if (questions.Count < 5)
            {
                ViewBag.ErrorMessage = "Yeterli sayıda quiz sorusu bulunmuyor. Lütfen daha sonra tekrar deneyin.";
                return View("ApplicationClosed");
            }

            ViewBag.Questions = questions;
            return View();
        }

        // POST: EmployeeApplication/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,PhoneNumber,Email,Experience,DesiredPosition,DesiredHourlyRate")] EmployeeApplication application, Dictionary<int, string> quizAnswers)
        {
            if (ModelState.IsValid)
            {
                // Get the questions for validation
                var questions = await _context.QuizQuestions
                    .Where(q => q.IsActive)
                    .OrderBy(q => Guid.NewGuid())
                    .Take(5)
                    .ToListAsync();

                // Calculate quiz score
                int correctAnswers = 0;
                var answersJson = new Dictionary<int, string>();

                foreach (var question in questions)
                {
                    if (quizAnswers.TryGetValue(question.Id, out string? userAnswer))
                    {
                        answersJson[question.Id] = userAnswer;
                        if (userAnswer.Equals(question.CorrectOption, StringComparison.OrdinalIgnoreCase))
                        {
                            correctAnswers++;
                        }
                    }
                }

                // Check if applicant passed the quiz (minimum 3/5 correct)
                if (correctAnswers < 3)
                {
                    ViewBag.ErrorMessage = $"Maalesef quiz testini geçemediniz. {correctAnswers}/5 doğru cevap verdiniz. En az 3 doğru cevap gereklidir.";
                    ViewBag.Questions = questions;
                    return View(application);
                }

                // Save application
                application.CorrectAnswers = correctAnswers;
                application.TotalQuestions = 5;
                application.QuizScore = (decimal)correctAnswers / 5 * 100;
                application.QuizAnswers = JsonSerializer.Serialize(answersJson);
                application.Status = ApplicationStatus.Pending;
                application.ApplicationDate = DateTime.Now;

                _context.Add(application);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Başvurunuz başarıyla alındı! Quiz sonucunuz: {correctAnswers}/5 doğru. Başvurunuz değerlendirilecek.";
                return RedirectToAction(nameof(Details), new { id = application.Id });
            }

            // If model state is invalid, reload questions
            var questionsForRetry = await _context.QuizQuestions
                .Where(q => q.IsActive)
                .OrderBy(q => Guid.NewGuid())
                .Take(5)
                .ToListAsync();

            ViewBag.Questions = questionsForRetry;
            return View(application);
        }

        // GET: EmployeeApplication/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.EmployeeApplications
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // GET: EmployeeApplication/Approve/5
        public async Task<IActionResult> Approve(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.EmployeeApplications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            // Check if we already have 4 employees
            var activeEmployeesCount = await _context.Employees.CountAsync(e => e.IsActive);
            if (activeEmployeesCount >= 4)
            {
                TempData["Error"] = "Maalesef şu anda 4 çalışanımız var ve yeni işe alım yapılamıyor.";
                return RedirectToAction(nameof(Details), new { id = id });
            }

            return View(application);
        }

        // POST: EmployeeApplication/Approve/5
        [HttpPost, ActionName("Approve")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveConfirmed(int id)
        {
            var application = await _context.EmployeeApplications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            // Check employee limit again
            var activeEmployeesCount = await _context.Employees.CountAsync(e => e.IsActive);
            if (activeEmployeesCount >= 4)
            {
                TempData["Error"] = "Maalesef şu anda 4 çalışanımız var ve yeni işe alım yapılamıyor.";
                return RedirectToAction(nameof(Index));
            }

            // Create employee from application
            var employee = new Employee
            {
                FirstName = application.FirstName,
                LastName = application.LastName,
                PhoneNumber = application.PhoneNumber,
                Email = application.Email,
                Position = application.DesiredPosition,
                HourlyRate = application.DesiredHourlyRate,
                HireDate = DateTime.Now,
                IsActive = true,
                DefaultWorkStart = new TimeSpan(9, 0, 0), // 09:00
                DefaultWorkEnd = new TimeSpan(18, 0, 0)   // 18:00
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Update application
            application.Status = ApplicationStatus.Approved;
            application.ReviewedDate = DateTime.Now;
            application.ReviewNotes = "İşe alındı.";
            application.EmployeeId = employee.Id;

            _context.Update(application);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"{application.FullName} başarıyla işe alındı!";
            return RedirectToAction(nameof(Index));
        }

        // GET: EmployeeApplication/Reject/5
        public async Task<IActionResult> Reject(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.EmployeeApplications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // POST: EmployeeApplication/Reject/5
        [HttpPost, ActionName("Reject")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectConfirmed(int id, string reviewNotes)
        {
            var application = await _context.EmployeeApplications.FindAsync(id);
            if (application != null)
            {
                application.Status = ApplicationStatus.Rejected;
                application.ReviewedDate = DateTime.Now;
                application.ReviewNotes = reviewNotes ?? "Uygun bulunmadı.";
                
                _context.Update(application);
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Başvuru reddedildi.";
            return RedirectToAction(nameof(Index));
        }

        // GET: EmployeeApplication/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.EmployeeApplications
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // POST: EmployeeApplication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var application = await _context.EmployeeApplications.FindAsync(id);
            if (application != null)
            {
                _context.EmployeeApplications.Remove(application);
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Başvuru silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
