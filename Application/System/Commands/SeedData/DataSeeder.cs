using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.System.Commands.SeedData
{
    public class DataSeeder
    {
        private readonly ISchoolContext _context;

        public DataSeeder(ISchoolContext context)
        {
            _context = context;
        }

        public async Task Seed()
        {
            if (_context.Students.Any())
            {
                return;
            }

            Student[] students = await AddStudents();

            Instructor[] instructors = await AddInstructors();

            Department[] departments = await AddDepartments(instructors);

            Course[] courses = await AddCourses(departments);

            await AddOfficeAssignments(instructors);

            await AddCourseAssignments(instructors, courses);

            await AddEnrollments(students, courses);
        }

        private async Task AddEnrollments(Student[] students, Course[] courses)
        {
            var enrollments = new Enrollment[]
            {
                new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alexander").Id,
                    CourseID = courses.Single(c => c.Title == "Chemistry" ).Id,
                    Grade = Grade.A
                },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alexander").Id,
                    CourseID = courses.Single(c => c.Title == "Microeconomics" ).Id,
                    Grade = Grade.C
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alexander").Id,
                    CourseID = courses.Single(c => c.Title == "Macroeconomics" ).Id,
                    Grade = Grade.B
                    },
                    new Enrollment {
                        StudentID = students.Single(s => s.LastName == "Alonso").Id,
                    CourseID = courses.Single(c => c.Title == "Calculus" ).Id,
                    Grade = Grade.B
                    },
                    new Enrollment {
                        StudentID = students.Single(s => s.LastName == "Alonso").Id,
                    CourseID = courses.Single(c => c.Title == "Trigonometry" ).Id,
                    Grade = Grade.B
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alonso").Id,
                    CourseID = courses.Single(c => c.Title == "Composition" ).Id,
                    Grade = Grade.B
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Anand").Id,
                    CourseID = courses.Single(c => c.Title == "Chemistry" ).Id
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Anand").Id,
                    CourseID = courses.Single(c => c.Title == "Microeconomics").Id,
                    Grade = Grade.B
                    },
                new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Barzdukas").Id,
                    CourseID = courses.Single(c => c.Title == "Chemistry").Id,
                    Grade = Grade.B
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Li").Id,
                    CourseID = courses.Single(c => c.Title == "Composition").Id,
                    Grade = Grade.B
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Justice").Id,
                    CourseID = courses.Single(c => c.Title == "Literature").Id,
                    Grade = Grade.B
                    }
            };

            foreach (Enrollment e in enrollments)
            {
                var enrollmentInDataBase = _context.Enrollments.Where(
                    s =>
                            s.Student.Id == e.StudentID &&
                            s.Course.Id == e.CourseID).SingleOrDefault();
                if (enrollmentInDataBase == null)
                {
                    _context.Enrollments.Add(e);
                }
            }
            await _context.SaveChangesAsync();
        }

        private async Task AddCourseAssignments(Instructor[] instructors, Course[] courses)
        {
            var courseInstructors = new Domain.Entities.CourseAssignment[]
            {
                new Domain.Entities.CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Chemistry" ).Id,
                    InstructorID = instructors.Single(i => i.LastName == "Kapoor").Id
                    },
                new Domain.Entities.CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Chemistry" ).Id,
                    InstructorID = instructors.Single(i => i.LastName == "Harui").Id
                    },
                new Domain.Entities.CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Microeconomics" ).Id,
                    InstructorID = instructors.Single(i => i.LastName == "Zheng").Id
                    },
                new Domain.Entities.CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Macroeconomics" ).Id,
                    InstructorID = instructors.Single(i => i.LastName == "Zheng").Id
                    },
                new Domain.Entities.CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Calculus" ).Id,
                    InstructorID = instructors.Single(i => i.LastName == "Fakhouri").Id
                    },
                new Domain.Entities.CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Trigonometry" ).Id,
                    InstructorID = instructors.Single(i => i.LastName == "Harui").Id
                    },
                new Domain.Entities.CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Composition" ).Id,
                    InstructorID = instructors.Single(i => i.LastName == "Abercrombie").Id
                    },
                new Domain.Entities.CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Literature" ).Id,
                    InstructorID = instructors.Single(i => i.LastName == "Abercrombie").Id
                    },
            };

            foreach (Domain.Entities.CourseAssignment ci in courseInstructors)
            {
                _context.CourseAssignments.Add(ci);
            }
            await _context.SaveChangesAsync();
        }

        private async Task AddOfficeAssignments(Instructor[] instructors)
        {
            var officeAssignments = new OfficeAssignment[]
            {
                new OfficeAssignment {
                    InstructorID = instructors.Single( i => i.LastName == "Fakhouri").Id,
                    Location = "Smith 17" },
                new OfficeAssignment {
                    InstructorID = instructors.Single( i => i.LastName == "Harui").Id,
                    Location = "Gowan 27" },
                new OfficeAssignment {
                    InstructorID = instructors.Single( i => i.LastName == "Kapoor").Id,
                    Location = "Thompson 304" },
            };

            foreach (OfficeAssignment o in officeAssignments)
            {
                _context.OfficeAssignments.Add(o);
            }
            await _context.SaveChangesAsync();
        }

        private async Task<Course[]> AddCourses(Department[] departments)
        {
            var courses = new Course[]
            {
                new Course {Id = 1050, Title = "Chemistry",      Credits = 3,
                    DepartmentID = departments.Single( s => s.Name == "Engineering").Id
                },
                new Course {Id = 4022, Title = "Microeconomics", Credits = 3,
                    DepartmentID = departments.Single( s => s.Name == "Economics").Id
                },
                new Course {Id = 4041, Title = "Macroeconomics", Credits = 3,
                    DepartmentID = departments.Single( s => s.Name == "Economics").Id
                },
                new Course {Id = 1045, Title = "Calculus",       Credits = 4,
                    DepartmentID = departments.Single( s => s.Name == "Mathematics").Id
                },
                new Course {Id = 3141, Title = "Trigonometry",   Credits = 4,
                    DepartmentID = departments.Single( s => s.Name == "Mathematics").Id
                },
                new Course {Id = 2021, Title = "Composition",    Credits = 3,
                    DepartmentID = departments.Single( s => s.Name == "English").Id
                },
                new Course {Id = 2042, Title = "Literature",     Credits = 4,
                    DepartmentID = departments.Single( s => s.Name == "English").Id
                },
            };

            foreach (Course c in courses)
            {
                _context.Courses.Add(c);
            }
            await _context.SaveChangesAsync();
            return courses;
        }

        private async Task<Department[]> AddDepartments(Instructor[] instructors)
        {
            var departments = new Department[]
            {
                new Department { Name = "English",     Budget = 350000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID  = instructors.Single( i => i.LastName == "Abercrombie").Id },
                new Department { Name = "Mathematics", Budget = 100000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID  = instructors.Single( i => i.LastName == "Fakhouri").Id },
                new Department { Name = "Engineering", Budget = 350000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID  = instructors.Single( i => i.LastName == "Harui").Id },
                new Department { Name = "Economics",   Budget = 100000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID  = instructors.Single( i => i.LastName == "Kapoor").Id }
            };

            foreach (Department d in departments)
            {
                _context.Departments.Add(d);
            }
            await _context.SaveChangesAsync();
            return departments;
        }

        private async Task<Instructor[]> AddInstructors()
        {
            var instructors = new Instructor[]
            {
                new Instructor { FirstMidName = "Kim",     LastName = "Abercrombie",
                    HireDate = DateTime.Parse("1995-03-11") },
                new Instructor { FirstMidName = "Fadi",    LastName = "Fakhouri",
                    HireDate = DateTime.Parse("2002-07-06") },
                new Instructor { FirstMidName = "Roger",   LastName = "Harui",
                    HireDate = DateTime.Parse("1998-07-01") },
                new Instructor { FirstMidName = "Candace", LastName = "Kapoor",
                    HireDate = DateTime.Parse("2001-01-15") },
                new Instructor { FirstMidName = "Roger",   LastName = "Zheng",
                    HireDate = DateTime.Parse("2004-02-12") }
            };

            foreach (Instructor i in instructors)
            {
                _context.Instructors.Add(i);
            }
            await _context.SaveChangesAsync();
            return instructors;
        }

        private async Task<Student[]> AddStudents()
        {
            var students = new Student[]
                        {
                new Student { FirstMidName = "Carson",   LastName = "Alexander",
                    EnrollmentDate = DateTime.Parse("2010-09-01") },
                new Student { FirstMidName = "Meredith", LastName = "Alonso",
                    EnrollmentDate = DateTime.Parse("2012-09-01") },
                new Student { FirstMidName = "Arturo",   LastName = "Anand",
                    EnrollmentDate = DateTime.Parse("2013-09-01") },
                new Student { FirstMidName = "Gytis",    LastName = "Barzdukas",
                    EnrollmentDate = DateTime.Parse("2012-09-01") },
                new Student { FirstMidName = "Yan",      LastName = "Li",
                    EnrollmentDate = DateTime.Parse("2012-09-01") },
                new Student { FirstMidName = "Peggy",    LastName = "Justice",
                    EnrollmentDate = DateTime.Parse("2011-09-01") },
                new Student { FirstMidName = "Laura",    LastName = "Norman",
                    EnrollmentDate = DateTime.Parse("2013-09-01") },
                new Student { FirstMidName = "Nino",     LastName = "Olivetto",
                    EnrollmentDate = DateTime.Parse("2005-09-01") }
                        };

            foreach (Student s in students)
            {
                _context.Students.Add(s);
            }
            await _context.SaveChangesAsync();
            return students;
        }
    }
}
