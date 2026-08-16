using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Domain.Entities;
using System.Threading.Tasks;
using System.Linq;

namespace Web.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly ISchoolContext _context;

        public EnrollmentsController(ISchoolContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var enrollments = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .ToListAsync();
            return View(enrollments);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(e => e.Id == id.Value);

            if (enrollment == null) return NotFound();

            return View(enrollment);
        }

        public IActionResult Create()
        {
            ViewData["CourseID"] = new SelectList(_context.Courses.ToList(), "Id", "Title");
            ViewData["StudentID"] = new SelectList(_context.Students.ToList(), "Id", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CourseID"] = new SelectList(_context.Courses.ToList(), "Id", "Title", enrollment.CourseID);
            ViewData["StudentID"] = new SelectList(_context.Students.ToList(), "Id", "FullName", enrollment.StudentID);
            return View(enrollment);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var enrollment = await _context.Enrollments.FindAsync(id.Value);
            if (enrollment == null) return NotFound();

            ViewData["CourseID"] = new SelectList(_context.Courses.ToList(), "Id", "Title", enrollment.CourseID);
            ViewData["StudentID"] = new SelectList(_context.Students.ToList(), "Id", "FullName", enrollment.StudentID);
            return View(enrollment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (id != enrollment.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(enrollment).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Enrollments.Any(e => e.Id == enrollment.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CourseID"] = new SelectList(_context.Courses.ToList(), "Id", "Title", enrollment.CourseID);
            ViewData["StudentID"] = new SelectList(_context.Students.ToList(), "Id", "FullName", enrollment.StudentID);
            return View(enrollment);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(e => e.Id == id.Value);
            if (enrollment == null) return NotFound();

            return View(enrollment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

