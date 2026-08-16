using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Application.Departments.Queries.GetDepartmentsOverview;
using Application.Departments.Queries.GetDepartmentDetails;
using Application.Departments.Commands.DeleteDepartment;
using Application.Departments.Queries.DeleteConfirmation;
using Application.Departments.Commands.CreateDepartment;
using Application.Instructors.Queries.GetInstructorsLookup;
using Application.Departments.Queries.GetUpdateDepartment;
using Application.Departments.Commands.UpdateDepartment;
using Domain.Entities;
using Application.Departments.Queries.GetCreateDepartment;

namespace Web.Controllers
{
    public class DepartmentsController : BaseController
    {
        public DepartmentsController()
        {
        }

        public async Task<IActionResult> Index()
        {
            var result = await Mediator.Send(new GetDepartmentsOverviewQuery());
            return View(result);
        }

        public async Task<IActionResult> Details(int? id)
        {
            {
                var result = await Mediator.Send(new GetDepartmentDetailsQuery(id));
                return View(result);
            }
        }

        public async Task<IActionResult> Create()
        {
            var result = await Mediator.Send(new GetInstructorLookupQuery());

            ViewData["InstructorID"] = new SelectList(result, "ID", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentID,Name,Budget,StartDate,InstructorID")] CreateDepartmentCommand command)
        {
            try
            {
                await Mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception)
            {
                var result = await Mediator.Send(new GetInstructorLookupQuery());
                ViewData["InstructorID"] = new SelectList(result, "ID", "FullName", command.InstructorID);

                return View(new CreateDepartmentVM
                {
                    Name = command.Name,
                    Budget = command.Budget,
                    StartDate = command.StartDate,
                    InstructorID = command.InstructorID
                });
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var result = await Mediator.Send(new GetUpdateDepartmentQuery(id));

            var instructors = await Mediator.Send(new GetInstructorLookupQuery());
            ViewData["InstructorID"] = new SelectList(instructors, "ID", "FullName", result.InstructorID);

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateDepartmentCommand command)
        {
            try
            {
                await Mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var vm = new UpdateDepartmentVM
                {
                    DepartmentID = command.DepartmentID.Value,
                    Name = command.Name,
                    Budget = command.Budget,
                    StartDate = command.StartDate,
                    InstructorID = command.InstructorID,
                    RowVersion = command.RowVersion
                };

                var exceptionEntry = ex.Entries.Single();
                var clientValues = (Department)exceptionEntry.Entity;
                var databaseEntry = exceptionEntry.GetDatabaseValues();
                if (databaseEntry == null)
                {
                    ModelState.AddModelError(string.Empty,
                        "Unable to save changes. The department was deleted by another user.");
                }
                else
                {
                    var databaseValues = (Department)databaseEntry.ToObject();

                    if (databaseValues.Name != clientValues.Name)
                    {
                        ModelState.AddModelError("Name", $"Current value: {databaseValues.Name}");
                    }
                    if (databaseValues.Budget != clientValues.Budget)
                    {
                        ModelState.AddModelError("Budget", $"Current value: {databaseValues.Budget:c}");
                    }
                    if (databaseValues.StartDate != clientValues.StartDate)
                    {
                        ModelState.AddModelError("StartDate", $"Current value: {databaseValues.StartDate:d}");
                    }
                    if (databaseValues.InstructorID != clientValues.InstructorID)
                    {
                        ModelState.AddModelError("InstructorID", $"Current value: {databaseValues.InstructorID}");
                    }

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                            + "was modified by another user after you got the original value. The "
                            + "edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink.");
                    vm.RowVersion = (byte[])databaseValues.RowVersion;
                    ModelState.Remove("RowVersion");
                }

                var instructorsLookup = await Mediator.Send(new GetInstructorLookupQuery());
                ViewData["InstructorID"] = new SelectList(instructorsLookup, "ID", "FullName", command.InstructorID);

                return View(vm);
            }
        }

        public async Task<IActionResult> Delete(int? id, bool? concurrencyError)
        {
            var result = await Mediator.Send(new GetDeleteDepartmentConfirmationQuery(id));

            if (concurrencyError.GetValueOrDefault())
            {
                ViewData["ConcurrencyErrorMessage"] = "The record you attempted to delete "
                    + "was modified by another user after you got the original values. "
                    + "The delete operation was canceled and the current values in the "
                    + "database have been displayed. If you still want to delete this "
                    + "record, click the Delete button again. Otherwise "
                    + "click the Back to List hyperlink.";
            }

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int DepartmentID)
        {
            try
            {
                await Mediator.Send(new DeleteDepartmentCommand(DepartmentID));
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction(nameof(Delete), new { concurrencyError = true, id = DepartmentID });
            }
        }
    }
}
