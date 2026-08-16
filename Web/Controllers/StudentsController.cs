using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Students.Queries.GetStudentsOverview;
using Application.Students.Queries.GetStudentDetails;
using Application.Students.Commands.CreateStudent;
using Application.Students.Commands.DeleteStudent;
using Application.Students.Queries.DeleteConfirmation;
using Application.Students.Commands.UpdateStudent;
using Application.Students.Queries.GetUpdateStudent;

namespace Web.Controllers
{
    public class StudentsController : BaseController
    {
        public StudentsController()
        {
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            var result = await Mediator.Send(new GetStudentsOverviewQuery(sortOrder, currentFilter, searchString, pageNumber));
            return View(result);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var result = await Mediator.Send(new GetStudentDetailsQuery(id));
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LastName,FirstName,EnrollmentDate")] CreateStudentCommand command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await Mediator.Send(command);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return View(command);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var result = await Mediator.Send(new GetUpdateStudentQuery(id));
            return View(result);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost([Bind("StudentID, LastName,FirstName,EnrollmentDate")] UpdateStudentCommand command)
        {
            try
            {
                await Mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }

            return View(command);
        }

        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            var result = await Mediator.Send(new GetDeleteConfirmationQuery(id));

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(result);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await Mediator.Send(new DeleteStudentCommand(id));

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
    }
}
