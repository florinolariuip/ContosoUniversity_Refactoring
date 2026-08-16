using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Application.Courses.Queries.GetCoursesOverview;
using Application.Courses.Queries.GetCourseDetails;
using Application.Courses.Commands.DeleteCourse;
using Application.Courses.Queries.DeleteConfirmation;
using Application.Courses.Commands.CreateCourse;
using Application.Departments.Queries.GetDepartmentsLookup;
using Application.Courses.Queries.GetEditCourse;
using Application.Courses.Commands.UpdateCourse;

namespace Web.Controllers
{
    public class CoursesController : BaseController
    {
        public CoursesController()
        {
        }

        public async Task<IActionResult> Index()
        {
            var result = await Mediator.Send(new GetCoursesOverviewQuery());
            return View(result);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var result = await Mediator.Send(new GetCourseDetailsQuery(id));
            return View(result);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDepartmentsDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseID,Credits,DepartmentID,Title")] CreateCourseCommand command)
        {
            try
            {
                await Mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception)
            {
                await PopulateDepartmentsDropDownList(command.DepartmentID);

                return View(new Domain.Entities.Course
                {
                    Id = command.CourseID,
                    Title = command.Title,
                    Credits = command.Credits,
                    DepartmentID = command.DepartmentID
                });
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var result = await Mediator.Send(new GetEditCourseQuery(id));

            await PopulateDepartmentsDropDownList(result.DepartmentID);

            return View(result);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(UpdateCourseCommand command)
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

                await PopulateDepartmentsDropDownList(command.DepartmentID);

                return View(new EditCourseVM 
                {
                    CourseID = command.CourseID.Value,
                    Title = command.Title,
                    Credits = command.Credits,
                    DepartmentID = command.DepartmentID,
                });
            }
        }

        private async Task PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            var result = await Mediator.Send(new GetDepartmentsLookupQuery());
            ViewBag.DepartmentID = new SelectList(result, "DepartmentID", "Name", selectedDepartment);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var result = await Mediator.Send(new GetDeleteCourseConfirmationQuery(id));
            return View(result);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await Mediator.Send(new DeleteCourseCommand(id));
            return RedirectToAction(nameof(Index));
        }
    }
}
