using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Instructors.Queries.GetInstructorDetails;
using Application.Instructors.Queries.GetInstructorsOverview;
using Application.Instructors.Commands.DeleteInstructor;
using Application.Instructors.Queries.DeleteConfirmation;
using Application.Instructors.Queries.GetCreateInstructor;
using Application.Instructors.Commands.CreateInstructor;
using Application.Instructors.Queries.GetUpdateInstructor;
using Application.Courses.Queries.GetCourseList;
using Application.Instructors.Commands.UpdateInstructor;
using Domain.Entities;
using Application.CourseAssignment.Commands;

namespace Web.Controllers
{
    public class InstructorsController : BaseController
    {
        public InstructorsController()
        {
        }

        public async Task<IActionResult> Index(int? id, int? courseID)
        {
            var result = await Mediator.Send(new GetInstructorsOverviewQuery(id,courseID));
            return View(result);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var result = await Mediator.Send(new GetInstructorDetailsQuery(id));
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,HireDate,LastName")] CreateInstructorCommand command)
        {
            try
            {
                await Mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View(new CreateInstructorVM
                {
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    HireDate = command.HireDate
                });
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var result = await Mediator.Send(new GetUpdateInstructorQuery(id));

            await PopulateAssignedCourseData(result.InstructorID);

            return View(result);
        }

        private async Task PopulateAssignedCourseData(int instructorID)
        {
            ViewData["Courses"] = await Mediator.Send(new GetCourseListQuery(instructorID));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateInstructorCommand command, string[] selectedCourses)
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

                await PopulateAssignedCourseData(command.InstructorID.Value);
                return View(new UpdateInstructorVM 
                {
                    InstructorID = command.InstructorID.Value,
                    LastName = command.LastName,
                    FirstName = command.FirstName,
                    HireDate = command.HireDate,
                    OfficeLocation = command.OfficeLocation
                });
            }
        }

        private async Task UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            await Mediator.Send(new UpdateCourseAssignmentsCommand(instructorToUpdate.Id, selectedCourses));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var result = await Mediator.Send(new GetDeleteInstructorConfirmationQuery(id));
            return View(result);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await Mediator.Send(new DeleteInstructorCommand(id));
            return RedirectToAction(nameof(Index));
        }
    }
}
